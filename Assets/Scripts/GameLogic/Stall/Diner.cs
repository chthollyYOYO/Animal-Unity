using Framework;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace GameLogic
{
    public enum DinerAction
    {
        路过,
        去点餐,
        候座,
        前往座位,
        等餐,
        干饭,
        离开,
    }

    public class Diner
    {
        public const string PATH = "Prefab/Diner";
        public string PATH_sit;
        public string PATH_walk;
        private List<int> OrderManual = new List<int> {3003, 3018, 3019, 3022};
        public float moveSpeed = 2f;
        public float animSpeed = 0.1f;
        public int dinerId;
        public int tableId;
        public int roleId;
        public int mealItemId;
        public bool isServed;
        public DinerAction currAction;
        public GameObject gameObject;
        private Transform top;
        private SpriteRenderer sr;
        private int animDirection = 1;
        private Point currPoint;
        private List<Point> targetPoints = new();
        private bool isMoving = false;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float totalDistance;
        private float currentDistance;
        private float eatingTime;
        private float timer;
        private UI_Component_Intent ui_intent;

        public float scale = 1;
        public bool isPause = false;

        public static Diner Create(int dinerId, int tableId = 0, int roleId = 307)
        {
            Diner diner = new();
            diner.dinerId = dinerId;
            diner.tableId = tableId;
            diner.roleId = roleId;
            diner.Init();
            return diner;
        }

        public void Init()
        {
            var prefab = ResourceManager.LoadAssetSync<GameObject>(PATH);
            gameObject = UnityEngine.Object.Instantiate(prefab);
            top = gameObject.transform.FindDeepChild("Top");
            sr = gameObject.GetComponent<SpriteRenderer>();

            PATH_sit = ConfigManager.GetRoleCfg(roleId).Sit;
            PATH_walk = ConfigManager.GetRoleCfg(roleId).Walk;
            sr.sprite = ResourceManager.LoadAssetSync<Sprite>(PATH_walk);

            GameMonoSingleton.Instance.onLogicUpdate += OnLogicUpdate;
            GameMonoSingleton.Instance.onLogicFixedUpdate += OnLogicFixedUpdate;
            GameMonoSingleton.Instance.onLateUpdate += OnLateUpdate;

            //ui_intent = UIPackage.CreateObject("Main", "Component_Intent") as UI_Component_Intent;
            //UI_Panel_Main.current.GetScene().AddChild(ui_intent);
            ui_intent = SceneUIManager.Instance.CreateUI<UI_Component_Intent>("Main", "Component_Intent");
            ui_intent.visible = false;

            InitAction();
            UpdateMovement(0);

            eatingTime = ConfigManager.GetGameCfg(0).DinerEatingTime;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(gameObject);
            StallSystem.Instance.OnDinerDestroy(dinerId);
            GameMonoSingleton.Instance.onLogicUpdate -= OnLogicUpdate;
            GameMonoSingleton.Instance.onLogicFixedUpdate -= OnLogicFixedUpdate;
            GameMonoSingleton.Instance.onLateUpdate -= OnLateUpdate;
            //if (ui_intent != null)
            //{
            //    ui_intent.Dispose();
            //    ui_intent=null;
            //}
            SceneUIManager.Instance.DestroyUI(ui_intent);
        }

        private void OnLogicUpdate(float deltaTime)
        {
            if (isPause)
                return;

            UpdateAction(deltaTime);
            UpdateAnimation();
        }

        private void OnLogicFixedUpdate(float deltaTime)
        {
            if (isPause)
                return;

            UpdateMovement(deltaTime);
        }

        private void OnLateUpdate()
        {
            if (ui_intent.visible)
                ui_intent.xy = UICamera.Instance.WorldPointToUI(top.position);
        }

        private void InitAction()
        {
            currPoint = PointManager.Instance.GetPointRandom(PointType.起点);
            currAction = DinerAction.去点餐;
            timer = 0;

            ui_intent.Update(0);

            gameObject.transform.position = currPoint.transform.position;
            StallSystem.Instance.Enqueue(dinerId);
        }

        private void UpdateAction(float deltaTime)
        {
            //移动中
            if (targetPoints.Count > 0)
                return;

            if (currAction == DinerAction.路过)
            {
                Destroy();
            }
            else if (currAction == DinerAction.去点餐)
            {
                int index = StallSystem.Instance.GetLiningIndex(dinerId);
                if (index == 0 && currPoint.Equal(PointType.排队点, 0))
                {
                    currAction = DinerAction.候座;
                    StallSystem.Instance.NotifyPeekReady();
                }
                else
                {
                    var targetPoint = PointManager.Instance.GetPoint(PointType.排队点, index);
                    targetPoints = PathFinderSystem.Instance.FindPath(currPoint, targetPoint);
                }
            }
            else if (currAction == DinerAction.候座)
            {
                
            }
            else if (currAction == DinerAction.前往座位)
            {
                currAction = DinerAction.等餐;
                sr.sprite = ResourceManager.LoadAssetSync<Sprite>(PATH_sit);
            }
            else if (currAction == DinerAction.等餐)
            {
                if (isServed)
                {
                    currAction = DinerAction.干饭;
                    timer = 0;
                    ui_intent.Update(2);
                }
            }
            else if (currAction == DinerAction.干饭)
            {
                timer += deltaTime;
                if (timer > eatingTime)
                {
                    currAction = DinerAction.离开;
                    var targetPoint = PointManager.Instance.GetPointRandom(PointType.起点);
                    targetPoints = PathFinderSystem.Instance.FindPath(currPoint, targetPoint);
                    DinerLeave();
                    sr.sprite = ResourceManager.LoadAssetSync<Sprite>(PATH_walk);
                }
            }
            else if (currAction == DinerAction.离开)
            {
                Destroy();
            }
        }

        public void AssignTable(int tableId)
        {
            this.tableId = tableId;

            if (Record.current.IsCompletedDialogue(1008))
            {
                int index = UnityEngine.Random.Range(0, OrderManual.Count);
                mealItemId = OrderManual[index];
            }
            else
            {
                mealItemId = 3018;
            }

            currAction = DinerAction.前往座位;
            Point targetPoint = PointManager.Instance.GetPoint(PointType.餐桌, tableId);
            targetPoints = PathFinderSystem.Instance.FindPath(currPoint, targetPoint);

            if (tableId != 0)
            {
                InteractableTipManager.Instance.UpdateTip();
                ui_intent.visible = true;
                ui_intent.Update(1, mealItemId);
            }
        }

        private void DinerLeave()
        {
            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.餐桌, tableId);
            var table = flag.GetComponent<InteractableObject_Table>();
            table.DinerLeave(tableId);
            //检查是否是正确的餐品
            if (table.itemCorrection)
            {
                //controller 3 =>笑脸
                ui_intent.Update(3, mealItemId);
            }
            else
            {
                //controller 4 =>哭脸
                ui_intent.Update(4, mealItemId);
            }

            if (table.itemCorrection)
                UltySystem.Instance.EnergyUp();
        }

        private void UpdateAnimation()
        {
            float scaleX = Mathf.Clamp(sr.transform.localScale.x - animDirection * animSpeed, -scale, scale);
            sr.transform.SetLocalScaleX(scaleX);
        }

        private void UpdateMovement(float deltaTime)
        { 
            // 如果没有目标点，直接返回
            if (targetPoints == null || targetPoints.Count == 0)
            {
                isMoving = false;
                return;
            }
            // 如果当前点为空，使用第一个目标点作为当前点
            if (currPoint == null)
            {
                currPoint = targetPoints[0];
                gameObject.transform.position = currPoint.transform.position;
                targetPoints.RemoveAt(0);
                return;
            }
            // 如果没有下一个目标点，返回
            if (targetPoints.Count == 0)
                return;
            // 如果未开始移动，初始化移动参数
            if (!isMoving)
            {
                Point nextPoint = targetPoints[0];

               // startPosition = currPoint.transform.position;
                startPosition = gameObject.transform.position;
                targetPosition = nextPoint.transform.position;
                totalDistance = Vector3.Distance(startPosition, targetPosition);
                currentDistance = 0f;
                isMoving = true;
                //更新动画方向
                float diff = targetPosition.x - startPosition.x;
                if (Mathf.Abs(diff) > 0)
                    animDirection = (int)Mathf.Sign(diff);
            }

            // 进行移动
            if (isMoving)
            {
                // 计算当前帧移动的距离
                float stepDistance = moveSpeed * deltaTime;
                currentDistance += stepDistance;

                // 检查是否到达目标点
                if (currentDistance >= totalDistance)
                {
                    // 精确到达终点
                    gameObject.transform.position = targetPosition;

                    // 更新当前点
                    currPoint = targetPoints[0];
                    targetPoints.RemoveAt(0);

                    // 重置移动状态
                    isMoving = false;
                }
                else
                {
                    // 计算插值比例
                    float t = currentDistance / totalDistance;
                    gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                }
            }
        }

        public void OnStallStop()
        {
            if (currAction == DinerAction.候座 || currAction == DinerAction.路过 || currAction == DinerAction.去点餐)
            {
                currAction = DinerAction.离开;
                var targetPoint = PointManager.Instance.GetPointRandom(PointType.起点);
                targetPoints = PathFinderSystem.Instance.FindPath(currPoint, targetPoint);

                isMoving = false;
            }
        }
    }
}
