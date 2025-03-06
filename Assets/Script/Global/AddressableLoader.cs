using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class AddressableLoader : MonoBehaviour
{
    private void Start()
    {
        
    }

    public async UniTaskVoid LoadAssetAsync(string address)
    {
        AsyncOperationHandle<SpriteAtlas> handle = Addressables.LoadAssetAsync<SpriteAtlas>(address);

        try
        {

        }
        catch (System.Exception ex) 
        {
            Debug.LogError($"Addressable 에셋 로딩 실패 : {ex}");
        }
    }


    // Update is called once per frame
    private void Update()
    {
        
    }
}
