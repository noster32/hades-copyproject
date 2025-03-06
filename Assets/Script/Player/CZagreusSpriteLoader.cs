using Cysharp.Threading.Tasks;
using UnityEngine;

public class CZagreusSpriteLoader : SpriteLoader
{
    #region Empty    

    public Sprite[][] ZagreusIdleSprites;

    public Sprite[][] ZagreusRunSprites;

    public Sprite[][] ZagreusStartSprites;
    public Sprite[][] ZagreusStopSprites;

    public Sprite[][] ZagreusDashSprites;
    public Sprite[][] ZagreusDashVFXSprites;

    #endregion

    #region Sword 

    public Sprite[][] ZagreusSwordIdle1Sprites;
    public Sprite[][] ZagreusSwordIdle2Sprites;

    public Sprite[][] ZagreusSwordRunSprites;
    public Sprite[][] ZagreusSwordStopSprites;

    public Sprite[][] ZagreusSwordAttack1Sprites;
    public Sprite[][] ZagreusSwordAttack2Sprites;
    public Sprite[][] ZagreusSwordAttack3Sprites;

    public Sprite[][] ZagreusSwordParrySprites;

    public Sprite[][] ZagreusSwordDashAttackSprites;

    public Sprite[][] ZagreusSwordReturnToIdle1Sprites;
    public Sprite[][] ZagreusSwordReturnToIdle2Sprites;

    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    protected override async UniTask InitializeSprites()
    {
        var loadTasks = new UniTask<Sprite[][]>[]
        {
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusIdleAddress, ZagreusSpriteFrame.ZagreusIdleDirection),                   //Idle
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusRunAddress, ZagreusSpriteFrame.ZagreusRunDirection),                     //Run
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusStartAddress, ZagreusSpriteFrame.ZagreusStartDirection),                 //Start
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusStopAddress, ZagreusSpriteFrame.ZagreusStopDirection),                   //Stop
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusDashAddress, ZagreusSpriteFrame.ZagreusDashDirection),                   //Dash
            LoadSpritesAsync(ZagreusSpriteFrame.ZagreusDashVFXAddress, ZagreusSpriteFrame.ZagreusDashVFXDirection),             //DashVFX
        };

        Sprite[][][] results = await UniTask.WhenAll(loadTasks);

        ZagreusIdleSprites = results[0];
        ZagreusRunSprites = results[1];
        ZagreusStartSprites = results[2];
        ZagreusStopSprites = results[3];
        ZagreusDashSprites = results[4];
        ZagreusDashVFXSprites = results[5];
    }
}
