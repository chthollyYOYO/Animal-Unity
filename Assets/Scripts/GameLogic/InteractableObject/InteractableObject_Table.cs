using Framework;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    /// <summary>
    /// 餐桌
    /// </summary>
    [RequireComponent(typeof(SceneFlag))]
    public class InteractableObject_Table : InteractableObject
    {
        public SceneFlag sceneFlag;
        public int needItemId;
        public Item item;
        public bool Clean;
        public int dinerId;

        public event UnityAction onItemChanged;

        private GameObject DirtyDishes;
        private GameObject MealItems;
        private Transform gold_pos;
        private SpriteRenderer item_sr;
        private Sprite sprite;
        private float LongPressDuration;
        private float CleanupMillisecond;
        public int currTable;
        public UI_Action_Gold ui_gold;
        public bool itemCorrection = false;

        public int mealsellprice;

        private int tempSoundId;
        bool cleanSoundPlaying;
        protected override void Start()
        {
            base.Start();
            sceneFlag = GetComponent<SceneFlag>();
            DirtyDishes = transform.Find("脏碗").gameObject;
            DirtyDishes.SetActive(false);
            Clean = true;
            CleanupMillisecond = ConfigManager.GetGameCfg(0).CleanupMillisecond / 1000f;

            MealItems = transform.Find("MealItem").gameObject;
            item_sr = MealItems.GetComponent<SpriteRenderer>();

            gold_pos = transform.Find("脏碗/Money").gameObject.transform;
            //ui_gold ??= UIPackage.CreateObject("Main", "Action_Gold") as UI_Action_Gold;
            //UI_Panel_Main.current.GetScene().AddChild(ui_gold);
            ui_gold = SceneUIManager.Instance.CreateUI<UI_Action_Gold>("Main", "Action_Gold");
            ui_gold.visible = false;
        }

        public void LateUpdate()
        {
            if(ui_gold != null)
                ui_gold.xy = UICamera.Instance.WorldPointToUI(gold_pos.position);
            //if(base.Selecting && Clean == true)
            //    UI_Panel_Main.current.ChangeInteractButtonStyle(0);
        }
        //public override void OnSelected()
        //{
        //    base.OnSelected();
        //    if (GetServeable(out _))
        //        UI_Panel_Main.current.ChangeInteractButtonStyle(3);
        //    else if (Clean == false)
        //        //桌面需要清理时，将交互UI变为长按清洁
        //        UI_Panel_Main.current.ChangeInteractButtonStyle(4);
        //}

        //public override void OnCancelSelected()
        //{
        //    base.OnCancelSelected();
        //    UI_Panel_Main.current.ChangeInteractButtonStyle(0);
        //}

        public override bool IsInteractable()
        {

            return !Clean || GetServeable(out _);
        }

        public override void OnInteract()
        {
            base.OnInteract();
            if (GetServeable(out var diner))
            {
                needItemId = diner.mealItemId;
                UIManager.Instance.ShowPanelSync<UI_Panel_Table, InteractableObject_Table>(this);
                //UI_Panel_Main.current.ChangeInteractButtonStyle(0);
            }
        }

        public bool TryAddItem(Item item)
        {
            var result = this.item == null;
            if (result)
            {
                this.item = item;              
                onItemChanged?.Invoke();           
            }
            return result;
        }

        public Item RemoveItem()
        {
            var result = item;
            if (result != null)
            {
                item = null;
                onItemChanged?.Invoke();
            }
            return result;
        }

        public void TryServe()
        {
            if (item != null)
                mealsellprice = ConfigManager.GetItemCfg(item.id).SellPrice;

            if (item != null && item.id == needItemId)//菜品正确
            {
                AudioManager.Instance.PlaySound("Music/jiaohu/shangduicai");
                var diner = StallSystem.Instance.GetDinerByTableId(sceneFlag.id);
                diner.isServed = true;
                itemCorrection = true;
                MealOnTable(item.id);
                RemoveItem();
            }
            else if (item != null && item.id != needItemId) //上错菜
            {
                AudioManager.Instance.PlaySound("Music/jiaohu/shangchuocai");
                var diner = StallSystem.Instance.GetDinerByTableId(sceneFlag.id);
                diner.isServed = true;
                itemCorrection = false;
                MealOnTable(item.id);
                RemoveItem();
            }

            //如果上错菜，只获得60%金额
            if (!itemCorrection)
                mealsellprice = Mathf.CeilToInt(0.6f * mealsellprice);
        }

        public bool GetServeable(out Diner diner)
        {
            diner = StallSystem.Instance.GetDinerByTableId(sceneFlag.id);
            return diner != null && diner.currAction == DinerAction.等餐;
        }

        public void DinerLeave(int tableID)
        {
            //吃完餐点，隐藏桌上餐点图片
            MealItems.SetActive(false);
            //吃完餐点，将脏碗图片显现
            DirtyDishes.SetActive(true);
            //开始播放金币旋转动画
            ui_gold.visible = true;
            ui_gold.GetTransition("Waiting").Play(); 
            //桌子清洁状态变更
            Clean = false;
            //将交互按钮UI变更为长按清洁，这样就算保持在桌面可交互范围内，顾客离开，也不需要重新select桌面才会更改UI
            //UI_Panel_Main.current.ChangeInteractButtonStyle(4);
            InteractableTipManager.Instance.UpdateTip();
            //暂存目前所处餐桌编号
            currTable = tableID;
            
        }

        public override void OnLongPress(float deltaTime)
        {
            base.OnLongPress(deltaTime);

            //检查是否在需要打扫状态
            if (Clean == false)
            {
                LongPressDuration += deltaTime;
                UpdateUI();
                if (!cleanSoundPlaying)
                {
                    tempSoundId = AudioManager.Instance.PlaySound("Music/jiaohu/shouzuozi");
                    cleanSoundPlaying = true;
                }


                if (LongPressDuration > CleanupMillisecond)
                {
                    OnLongPressEnd();
                    //打扫完成，将桌面状态更新为clean状态
                    Clean = true;
                    //将脏盘的object隐藏
                    DirtyDishes.SetActive(false);
                    //将目前被清洁的桌子ID传给stallsystem用于更新桌位状态
                    StallSystem.Instance.TableClean(currTable);
                    //打扫完成，获得金币，播放完成后，关闭
                    ui_gold.GetTransition("Get")?.Play(()=>
                        {
                            AudioManager.Instance.PlaySound("Music/jiaohu/jingbi");
                            ui_gold.visible = false;
                        });                   
                }
            }
        }


        public override void OnLongPressEnd()
        {
            base.OnLongPressEnd();
            LongPressDuration = 0;
            UpdateUI();
            AudioManager.Instance.StopSound(tempSoundId);

            cleanSoundPlaying = false;
        }

        private void UpdateUI()
        {
            UI_Panel_Main.current.OnLongPressInteract(LongPressDuration / CleanupMillisecond);
        }

        public void MealOnTable(int itemId)//渲染桌上对应餐点
        {
            var url = ConfigManager.GetItemCfg(itemId).Icon;
            if (sprite != null)
                Destroy(sprite);
            sprite = Helper.CreateSpriteByUrl(url, new Vector2(0.5f, 0.5f), 200);
            item_sr.sprite = sprite;
        }

        //void Destroy()
        //{
        //    if (sprite != null)
        //        UnityEngine.Object.Destroy(sprite);
        //}

        private void OnDestroy()
        {
            //UnityEngine.Object.Destroy(DirtyDishes);
            //UnityEngine.Object.Destroy(gold_pos);
            if (sprite != null)
                Destroy(sprite);
            //if (ui_gold != null)
            //{
            //    ui_gold.Dispose();
            //    ui_gold = null;
            //}
            SceneUIManager.Instance.DestroyUI(ui_gold);
        }
    }
}
