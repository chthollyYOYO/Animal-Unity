using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InteractableOutlineRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material compositeMaterial;

        public RenderPassEvent renderPassEvent =
            RenderPassEvent.AfterRenderingTransparents;
    }

    public Settings settings = new Settings();

    private InteractableOutlinePass pass;

    public override void Create()
    {
        pass = new InteractableOutlinePass(settings)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(
        ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (settings.compositeMaterial == null)
            return;

        // 只在 Base Camera 执行，避免 UI Camera / Stage Camera 也执行描边
        if (renderingData.cameraData.renderType != CameraRenderType.Base)
            return;

        pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(pass);
    }

    private class InteractableOutlinePass : ScriptableRenderPass
    {
        private readonly Settings settings;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public InteractableOutlinePass(Settings settings)
        {
            this.settings = settings;
            tempTexture.Init("_TempInteractableOutlineTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(
            ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            CommandBuffer cmd =
                CommandBufferPool.Get("Interactable Outline");

            RenderTextureDescriptor descriptor =
                renderingData.cameraData.cameraTargetDescriptor;

            descriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT(
                tempTexture.id,
                descriptor,
                FilterMode.Bilinear);

            cmd.Blit(source, tempTexture.Identifier());

            cmd.SetGlobalTexture(
                "_BlitTexture",
                tempTexture.Identifier());

            cmd.Blit(
                tempTexture.Identifier(),
                source,
                settings.compositeMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null)
                return;

            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }
}