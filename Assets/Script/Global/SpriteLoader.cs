using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

[DefaultExecutionOrder(-1)]
public abstract class SpriteLoader : MonoBehaviour
{
    public UniTask InitializationTask { get; private set; }

    protected virtual void Awake()
    {
        InitializationTask = InitializeSpritesAsync();
    }

    private async UniTask InitializeSpritesAsync()
    {
        await InitializeSprites();
    }

    protected abstract UniTask InitializeSprites();

    protected async UniTask<Sprite[][]> LoadSpritesAsync(string baseAddress, int directionCount)
    {
        var loadTasks = Enumerable.Range(1, directionCount)
            .Select(dir => LoadDirectionSprites(baseAddress + $"{dir}"))
            .ToArray();

        return await UniTask.WhenAll(loadTasks);
    }

    //스프라이트 로드
    private async UniTask<Sprite[]> LoadDirectionSprites(string address)
    {
        try
        {
            var loadOperation = Addressables.LoadAssetAsync<SpriteAtlas>(address);
            SpriteAtlas atlas = await loadOperation.ToUniTask();

            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);

            if (sprites.Length == 0)
            {
                Debug.Log(address + " 로딩 실패");
            }
            else
            {
                Debug.Log(sprites[0].name);
            }

            Addressables.Release(loadOperation);
            return sprites.OrderBy(s =>
            {
                string cleanName = s.name.Replace("(Clone)", "");

                //숫자 부분 추출 ("Zagreus_sprite_1_0" -> [1, 0])
                string[] parts = cleanName.Split('_');
                int frameNumber = int.Parse(parts[3]);
                return frameNumber;
            }).ToArray();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Sprite 로딩 오류 발생: {e}");
            return null;
        }
    }
}
