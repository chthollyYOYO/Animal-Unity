using Framework;
using Sirenix.OdinInspector;
using System;
using UI;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class PlayerController : MonoBehaviour, IInputController
    {
        public static PlayerController Instance;

        [Header("移动设置")]
        [LabelText("速度")]
        public float moveSpeed = 5f;
        [LabelText("加速度")]
        public float acceleration = 10f;
        [LabelText("减速度")]
        public float deceleration = 10f;
        [LabelText("奔跑速度")]
        public float runningSpeed = 8f;

        [Header("动画设置")]
        [LabelText("速度")]
        public float animSpeed = 0.1f;

        public Vector2 movementInput { get; private set; }
        public Vector2 currentVelocity { get; private set; }
        public Vector2 targetVelocity { get; private set; }
        public Vector2Int position { get; private set; }
        public Vector2Int direction { get; private set; } = Vector2Int.right;

        int IInputController.sortingOrder => -10;

        private Transform tf_top;
        private Transform tf_right;

        // 组件引用
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        private int animDirection = 1;

        //声明体力条UI
        private UI_ProgressBar_Run ui_runningBar;
        //是否按下shift+移动方向开始跑步
        private bool isRunning;
        //最大体力值
        private float EpMax;
        //奔跑状态每秒体力消耗
        private float EpCostPerSecond;
        //非奔跑状态每秒体力消耗
        private float EpRecoverPerSecond;
        //当前体力条余量
        private float EpCurrent;

        public event Action onInputUpdate;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();

            tf_top = transform.FindDeepChild("Top");
            tf_right = transform.FindDeepChild("Right");

            InitScene();
            InitInteract();
            InitQTE();

            //获得奔跑体力条相关参数
            EpMax = ConfigManager.GetGameCfg(0).EpMax;
            EpRecoverPerSecond = ConfigManager.GetGameCfg(0).EpRecoverPerSecond;
            EpCostPerSecond = ConfigManager.GetGameCfg(0).EpCostPerSecond;

            //体力条UI组件
            ui_runningBar = SceneUIManager.Instance.CreateUI<UI_ProgressBar_Run>("Main", "ProgressBar_Run");
            ui_runningBar.visible = false;
            //使初始体力为满
            EpCurrent = EpMax;
        }

        private void OnDestroy()
        {
            Instance = null;
            SceneUIManager.Instance.DestroyUI(ui_runningBar);
            DestroyQTE();
        }

        private void OnEnable()
        {
            InputSystem.Instance.RegisterController(this);
        }

        private void OnDisable()
        {
            InputSystem.Instance.UnRegisterController(this);
        }

        private void Update()
        {
            // 更新动画
            UpdateAnimation();

            UpdatePosition();

            UpdateDirection();
        }

        private void FixedUpdate()
        {
            // 处理移动（放在FixedUpdate中以保证物理稳定性）
            HandleMovement();
        }

        private void LateUpdate()
        {
            ui_runningBar.xy = UICamera.Instance.WorldPointToUI(tf_top.position);
        }

        public void OnInputUpdate(float deltaTime)
        {
            HandleInput_Move();

            HandleInteract(deltaTime);
            HandleUtlyPress();

            onInputUpdate?.Invoke();
        }

        private void HandleInput_Move()
        {
            // 获取键盘输入 (WASD 或 方向键)
            movementInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // 归一化对角线移动，防止速度过快
            if (movementInput.magnitude > 1f)
            {
                movementInput.Normalize();
            }

            // 计算目标速度
            targetVelocity = movementInput * moveSpeed;
            //检查是否开始跑步
            isRunning = movementInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift);

            if (isRunning)
            {
                //体力条消耗
                EpCurrent = EpCurrent - EpCostPerSecond * Time.deltaTime;
                //边界限制
                EpCurrent = Mathf.Clamp(EpCurrent, 0, EpMax);
                //显现体力条
                ui_runningBar.visible = true;
                //根据体力消耗改变体力条
                ui_runningBar.Update(EpCurrent);

                if (EpCurrent != 0) 
                {                
                    //计算奔跑目标速度
                    targetVelocity = movementInput * runningSpeed;
                }
            }
            else 
            {
                //体力条回复
                EpCurrent = EpCurrent + EpRecoverPerSecond * Time.deltaTime;
                //边界限制
                EpCurrent = Mathf.Clamp(EpCurrent, 0, EpMax);
                ui_runningBar.Update(EpCurrent);

                if (EpCurrent == EpMax) 
                {
                    //当体力条回复为满时，体力条消失
                    ui_runningBar.visible = false;
                }

            }
        }
        private void HandleUtlyPress()
        {
            if (StallSystem.Instance.isStalling && Input.GetKey(KeyCode.R))
            {
                UltySystem.Instance.UltyPress();
            }
        }

        /// <summary>
        /// 处理角色移动
        /// </summary>
        private void HandleMovement()
        {
            // 根据是否有输入选择加速或减速
            float accelerationRate = movementInput.magnitude > 0.1f ? acceleration : deceleration;

            // 平滑过渡当前速度到目标速度
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);

            // 应用速度到刚体
            rb.velocity = currentVelocity;
        }

        private void UpdateAnimation()
        {
            float scaleX = Mathf.Clamp(sr.transform.localScale.x + animDirection * animSpeed, -1f, 1f);
            sr.transform.SetLocalScaleX(scaleX);
        }

        private void UpdatePosition()
        {
            position = Vector2Int.FloorToInt(transform.position);
        }

        private void UpdateDirection()
        {
            // 如果输入向量接近零，不改变方向
            if (movementInput.magnitude < 0.01f)
            {
                return;
            }

            // 获取绝对值
            float absX = Mathf.Abs(movementInput.x);
            float absY = Mathf.Abs(movementInput.y);

            // 比较哪个分量的绝对值更大
            if (absX > absY)
            {
                // X轴为主方向
                direction = new Vector2Int((int)Mathf.Sign(movementInput.x), 0);
            }
            else if (absY > absX)
            {
                // Y轴为主方向
                direction = new Vector2Int(0, (int)Mathf.Sign(movementInput.y));
            }
            else
            {
                // 绝对值相等时（如(1,1)），优先选择X轴或根据需求处理
                // 这里选择X轴作为默认
                direction = new Vector2Int((int)Mathf.Sign(movementInput.x), 0);
            }

            if (Mathf.Abs(movementInput.x) > 1e-4f)
            {
                animDirection = (int)Mathf.Sign(movementInput.x);
            }
        }
    }
}