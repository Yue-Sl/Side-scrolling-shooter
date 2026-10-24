using Godot;
using Godot.Collections;
using System;

public partial class Player : CharacterBody2D
{
    AnimationPlayer anim;
    Sprite2D torso;
    Sprite2D leg;
    Sprite2D marker;
    Timer shootTimer;

    [Signal] public delegate void ShootEventHandler(Vector2 pos, Vector2 dir);

    Vector2 velocity = Vector2.Zero;
    Vector2 mouseDir;
    float speed = 100;
    float jump_strength = 250;
    int gravity = 10;

    bool isGround = true;
    bool canShoot = true;

    public override void _Ready()
    {
        // 获取动画控制器
        anim = GetNode<AnimationPlayer>("AnimationPlayer");

        // 获取Sprite
        torso = GetNode<Sprite2D>("torso");
        leg = GetNode<Sprite2D>("leg");
        marker = GetNode<Sprite2D>("cross");

        // 获取计时器
        shootTimer = GetNode<Timer>("shootTimer");

        // 订阅事件
        shootTimer.Timeout += OnShootTimerOut;

    }


    public override void _ExitTree()
    {
        // 取消订阅事件
        shootTimer.Timeout -= OnShootTimerOut;
    }

    public override void _PhysicsProcess(double delta)
    {
        // 获取水平方向输入
        velocity.X = Input.GetAxis("left","right") * speed;

        // 检测是否处于地面
        isGround = IsOnFloor();

        // 获取跳跃输入
        if (Input.IsActionJustPressed("jump") && isGround)
        {
            velocity.Y = -jump_strength;
        }

        // 获取鼠标方位
        mouseDir = GetLocalMousePosition().Normalized();

        // 获取射击输入
        if (Input.IsActionPressed("shoot") && canShoot)
        {
            canShoot = false;
            EmitSignal(SignalName.Shoot, this.Position, mouseDir);
            var tween = CreateTween();
            tween.TweenProperty(marker, "scale", new Vector2(0.1f,0.1f), 0.1);
            tween.TweenProperty(marker, "scale", new Vector2(0.2f,0.2f), 0.1);
            shootTimer.Start();
        }

        // 调整准心位置
        marker.Position = GetLocalMousePosition();

        // 应用移动
        Velocity = velocity;
        ApplyGravity();
        ApplyAnim();
        MoveAndSlide();
    }

    void ApplyGravity()
    {
        velocity.Y += gravity;
    }

    // 射击计时器
    void OnShootTimerOut()
    {
        canShoot = true;
    }


    // 应用动画
    void ApplyAnim()
    {
        if (!isGround)
        {
            anim.CurrentAnimation = "jump";
        }
        else
        {
            leg.FlipH = velocity.X < 0;
            anim.CurrentAnimation = velocity.X == 0 ? "idle" : "run";
        }
        int f;

        switch (new Vector2(Mathf.RoundToInt(mouseDir.X), Mathf.RoundToInt(mouseDir.Y)))
        {
            case (1,0):
                f = 0;
                break;
            case (1,1):
                f = 1;
                break;
            case (0,1):
                f = 2;
                break;
            case (-1, 1):
                f = 3;
                break;
            case (-1, 0):
                f = 4;
                break;
            case (-1, -1):
                f = 5;
                break;
            case (0, -1):
                f = 6;
                break;
            case (1, -1):
                f = 7;
                break;
            default: f = 0; break;
        }

        torso.Frame = f;

    }
}
