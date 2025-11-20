using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderFeature : ScriptableRendererFeature
{
    class OutlineRenderPass : ScriptableRenderPass
    {
        private Material outlineMaterial;
        private RenderTargetIdentifier source;
        [System.Obsolete]
        private RenderTargetHandle tempTexture;

        [System.Obsolete]
        public OutlineRenderPass(Material material)
        {
            this.outlineMaterial = material;
            tempTexture.Init("_TempOutlineTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        }

        [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            CommandBuffer cmd = CommandBufferPool.Get("Outline Effect");

            cmd.Blit(source, tempTexture.Identifier(), outlineMaterial, 0);
            cmd.Blit(tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void FrameCleanup(CommandBuffer cmd)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    public Shader outlineShader;
    private Material outlineMaterial;
    private OutlineRenderPass outlinePass;

    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void Create()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        if (outlineShader == null) return;

        outlineMaterial = CoreUtils.CreateEngineMaterial(outlineShader);
        outlinePass = new OutlineRenderPass(outlineMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        if (outlinePass != null)
        {
            outlinePass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(outlinePass);
        }
    }
}
