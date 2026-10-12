using cfg;
using Cysharp.Threading.Tasks;
using Framework;
using System;
using System.Collections.Generic;

namespace GameLogic
{
    public partial class DialogueSystem : Singleton<DialogueSystem>
    {
        //<groupId, cfgsInGroup>
        public Dictionary<int, List<DialogueCfg>> cfgs = new();

        private List<DialogueCfg> currentGroup;
        private DialogueAction currentAction;
        private DialogueOption currentOption;
        //private bool isAlreadyExecuteOption;
        private DialogueOptionAction currentOptionAction;

        private List<DialogueAction> waittingStopActions = new();
        private InputController_UI controller = new();

        public bool isDialoging = false;

        public void Initialize()
        {
            var cfgs = ConfigManager.GetDialogueCfgList();
            int groupId = 0;
            foreach (var cfg in cfgs)
            {
                if (cfg.GroupId > 0)
                    groupId = cfg.GroupId;

                if (!this.cfgs.TryGetValue(groupId, out var list))
                    list = this.cfgs[groupId] = new();
                list.Add(cfg);
            }

            controller.sortingOrder = -1;
        }

        public void StartDialogue(int groupId)
        {
            CacheSceneBgm();
            StopDialogue();
            if (cfgs.TryGetValue(groupId, out var list))
            {
                isDialoging = true;
                currentGroup = list;
                ExecuteDialogue(currentGroup[0]);

                GameMonoSingleton.Instance.onUpdate += OnUpdate;
                InputSystem.Instance.RegisterController(controller);
            }
            else
            {
                LogManager.Error($"不存在对话组:{groupId}");
            }
        }

        public async UniTask StartDialogueTask(int groupId)
        {
            StartDialogue(groupId);
            await UniTask.WaitUntil(() => !isDialoging);
        }

        public void StopDialogue()
        {
            int groupId = currentGroup == null || currentGroup.Count == 0 ? 0 : currentGroup[0].GroupId;
            StopCurrent();

            foreach (var action in waittingStopActions)
                action.OnStop();
            waittingStopActions.Clear();

            GameMonoSingleton.Instance.onUpdate -= OnUpdate;
            InputSystem.Instance.UnRegisterController(controller);
            PlaySceneBgm();

            if (groupId > 0)
                Record.current.AddCompletedDialogue(groupId);

            isDialoging = false;
        }

        private void ExecuteDialogue(DialogueCfg cfg)
        {
            if (DialogueOption.IsOption(cfg))
            {
                CreateOption(cfg);
                currentOption.Execute();
            }
            else
            {
                currentAction = DialogueActionCreator.Create(cfg);
                if (currentAction == null)
                {
                    LogManager.Error($"未处理的对话类型：{cfg.Action} {cfg.Param1}");
                    TryExecuteNextDialogue(cfg);
                }
                else
                {
                    currentAction.Execute();

                    if (currentAction.isNeedStop)
                        waittingStopActions.Add(currentAction);

                    //if (currentAction.isShowOption.HasValue)
                    //{
                    //    var nextCfg = GetNextCfgUseContentId(cfg);
                    //    CreateOption(nextCfg);
                    //    isAlreadyExecuteOption = false;
                    //}
                    CheckAction();
                }
            }
        }

        private void CreateOption(DialogueCfg cfg)
        {
            currentOption = null;
            bool isVaild;
            do
            {
                isVaild = cfg != null
                    && cfg.GroupId == 0
                    && DialogueOption.IsOption(cfg)
                    ;

                if (isVaild)
                {
                    currentOption ??= new();
                    currentOption.AddCfg(cfg);
                    cfg = GetNextCfgUseUniqueId(cfg);
                }
            }
            while (isVaild);
        }

        private void TryExecuteNextDialogue(DialogueCfg currentCfg)
        {
            StopCurrent();
            var nextCfg = GetNextCfgUseContentId(currentCfg);
            if (nextCfg == null)
            {
                StopDialogue();
            }
            else
            {
                ExecuteDialogue(nextCfg);
            }
        }

        private DialogueCfg GetNextCfgUseUniqueId(DialogueCfg currentCfg)
        {
            var nextCfg = ConfigManager.GetDialogueCfg(currentCfg.Id + 1, false);
            if (nextCfg != null && nextCfg.GroupId > 0)
                nextCfg = null;
            return nextCfg;
        }

        private DialogueCfg GetNextCfgUseContentId(DialogueCfg currentCfg)
        {
            int nextContentId = currentCfg.NextContentId;
            if (nextContentId == 0)
                return GetNextCfgUseUniqueId(currentCfg);

            var nextCfg = currentGroup.Find(it => it.ContentId == nextContentId);
            return nextCfg;
        }

        private void StopCurrent()
        {
            if (currentAction != null)
            {
                waittingStopActions.RemoveAll(it => it.IsMeetStopCondition(currentAction.cfg));
                currentAction.OnCompleted();
                currentAction = null;
            }

            currentOption?.Stop();
            currentOption = null;

            currentOptionAction?.Stop();
            currentOptionAction = null;
        }

        private void CheckAction()
        {
            if (currentAction != null)
            {
                if (currentOption == null)
                {
                    //没有选项时，动作完成后继续
                    if (currentAction.isFinished)
                    {
                        var currentCfg = currentAction.cfg;
                        TryExecuteNextDialogue(currentCfg);
                    }
                }
                else
                {
                    ////等待动作执行到显示选项
                    //if (currentAction.isShowOption.Value && !isAlreadyExecuteOption)
                    //{
                    //    isAlreadyExecuteOption = true;
                    //    currentOption.Execute();
                    //}
                }
            }

            //等待选择选项
            if (currentOption != null)
            {
                if (currentOption.chooseIndex >= 0)
                {
                    DialogueCfg GetCurrentCfg() => currentOption.cfgs[currentOption.chooseIndex];
                    //创建选项动作并执行
                    if (currentOptionAction == null)
                    {
                        currentOptionAction = DialogueOptionActionCreator.Create(GetCurrentCfg());
                        if (currentOptionAction != null)
                        {
                            currentOption.Stop();
                            currentOptionAction.Execute();
                        }
                        else
                        {
                            TryExecuteNextDialogue(GetCurrentCfg());
                        }
                    }
                    //等待选项动作完成
                    else
                    {
                        if (currentOptionAction.isFinished)
                        {
                            currentOptionAction.OnCompleted();
                            TryExecuteNextDialogue(GetCurrentCfg());
                        }
                    }
                }
            }
        }

        private void OnUpdate(float _)
        {
            CheckAction();
        }
    }
}