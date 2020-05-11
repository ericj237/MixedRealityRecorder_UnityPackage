using UnityEngine;
using UnityEngine.UI;

namespace MRR.Controller
{

    public class MrrUiController : MonoBehaviour
    {

        //public MrrAppManager appManager;

        public RawImage screenVirtualCamera;
        public RawImage screenForegroundMask;

        public void SetScreenVirtualCamera(RenderTexture colorTexture)
        {
            screenVirtualCamera.texture = colorTexture;
        }

        public void SetScreenForegroundMask(RenderTexture foregroundMaskTexture)
        {
            screenForegroundMask.texture = foregroundMaskTexture;
        }

        public void SetScreenForegroundMask(Texture2D foregroundMaskTexture)
        {
            screenForegroundMask.texture = foregroundMaskTexture;
        }
    }
}