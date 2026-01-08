using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blah : ScriptableRendererFeature
{
    class WipeStencilPass : ScriptableRenderPass
    {
        private Material mat;
        private RenderTargetIdentifier cameraColorTarget;

        public WipeStencilPass(Material m)
        {
            mat = m;
            renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // SAFE: this is the correct place to get the camera target in URP 6000+
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (mat == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("WipeStencilMask");

            // Bind the camera color target (we only write stencil)
            cmd.SetRenderTarget(
                cameraColorTarget,
                RenderBufferLoadAction.Load,
                RenderBufferStoreAction.Store,
                RenderBufferLoadAction.Load,
                RenderBufferStoreAction.Store
            );

            // Draw fullscreen quad with stencil-writing shader
            CoreUtils.DrawFullScreen(cmd, mat);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material wipeStencilMaterial;
    private WipeStencilPass pass;

    public override void Create()
    {
        pass = new WipeStencilPass(wipeStencilMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (wipeStencilMaterial == null)
            return;

        renderer.EnqueuePass(pass);
    }
}