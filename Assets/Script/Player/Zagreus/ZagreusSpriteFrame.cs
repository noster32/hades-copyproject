public static class ZagreusSpriteFrame
{
    #region Empty   
    
    //Idle
    public static int ZagreusIdleFrame { get; internal set; } = 40;
    public static int ZagreusIdleDirection { get; internal set; } = 32;
    public static string ZagreusIdleAddress { get; internal set; } = "ZagreusSprite/Empty/Idle_Direction_";

    //Run
    public static int ZagreusRunFrame { get; internal set; } = 16;
    public static int ZagreusRunDirection { get; internal set; } = 64;
    public static string ZagreusRunAddress { get; internal set; } = "ZagreusSprite/Empty/Run_Direction_";

    //Start
    public static int ZagreusStartFrame { get; internal set; } = 16;
    public static int ZagreusStartDirection { get; internal set; } = 64;
    public static string ZagreusStartAddress { get; internal set; } = "ZagreusSprite/Empty/Start_Direction_";

    //Stop
    public static int ZagreusStopFrame { get; internal set; } = 16;
    public static int ZagreusStopDirection { get; internal set; } = 32;
    public static string ZagreusStopAddress { get; internal set; } = "ZagreusSprite/Empty/Stop_Direction_";

    //Dash
    public static int ZagreusDashFrame { get; internal set; } = 30;
    public static int ZagreusDashDirection { get; internal set; } = 16;
    public static string ZagreusDashAddress { get; internal set; } = "ZagreusSprite/Empty/Dash_Direction_";

    //DashVFX
    public static int ZagreusDashVFXFrame { get; internal set; } = 13;
    public static int ZagreusDashVFXDirection { get; internal set; } = 16;
    public static string ZagreusDashVFXAddress { get; internal set; } = "ZagreusSprite/Empty/DashVFX_Direction_";

    #endregion


    //Attack
    public static int ZagreusAttackFrame { get; internal set; } = 16;
    public static int ZagreusAttackDirection { get; internal set; } = 32;
    public static string ZagreusAttackAddress { get; internal set; } = "ZagreusSprite/Empty/Attack_Direction_";
}
