using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class DitherEffectRendererFeature : ScriptableRendererFeature
{
    class DitherEffectPass : ScriptableRenderPass
    {
        const string m_PassName = "DitherEffectPass";
        Material m_BlitMaterial;

        public void Setup(Material mat)
        {
            m_BlitMaterial = mat;
            requiresIntermediateTexture = true;
        }
        
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var stack = VolumeManager.instance.stack;
            var ditherVolume = stack.GetComponent<DitheringVolumeComponent>();

            if (ditherVolume == null || !ditherVolume.IsActive())
            {
                return;
            }

            var resourceData = frameData.Get<UniversalResourceData>();

            //�츮�� 
            if(resourceData.isActiveTargetBackBuffer)
            {
                return;
            }

            var source = resourceData.activeColorTexture;
            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = $"CameraColor-{m_PassName}";
            destinationDesc.clearBuffer = false;

            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            RenderGraphUtils.BlitMaterialParameters para = new RenderGraphUtils.BlitMaterialParameters(source, destination, m_BlitMaterial, 0);
            renderGraph.AddBlitPass(para, passName: passName);

            resourceData.cameraColor = destination;
        }
    }

    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRendering;
    public Material material;

    DitherEffectPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new DitherEffectPass();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = injectionPoint;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(material == null)
        {
            Debug.LogWarning("Material is not assigned. Dither effect will not be applied.");
            return;
        }

        m_ScriptablePass.Setup(material);
        renderer.EnqueuePass(m_ScriptablePass);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && material != null)
        {
            if (Application.isPlaying)
            {
                Destroy(material);
            }
            else
            {
                DestroyImmediate(material);
            }
        }
    }
}
