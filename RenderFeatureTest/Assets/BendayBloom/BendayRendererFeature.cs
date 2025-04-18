using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Experimental.Rendering;

public class BendayRendererFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        Material m_BloomMaterial;
        Material m_CompositeMaterial;

        public CustomRenderPass(Material bloomMat, Material compositeMat)
        {
            m_BloomMaterial = bloomMat;
            m_CompositeMaterial = compositeMat;
        }
    }

    CustomRenderPass m_RenderPass;

    [SerializeField] Shader m_BloomShader;
    [SerializeField] Shader m_CompositeShader;

    Material m_BloomMaterial;
    Material m_CompositeMaterial;

    [SerializeField] RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingOpaques;

    public override void Create()
    {
        m_BloomMaterial = CoreUtils.CreateEngineMaterial(m_BloomShader);
        m_CompositeMaterial = CoreUtils.CreateEngineMaterial(m_CompositeShader);

        m_RenderPass = new CustomRenderPass(m_BloomMaterial, m_CompositeMaterial);
        m_RenderPass.renderPassEvent = injectionPoint;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_RenderPass);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CoreUtils.Destroy(m_BloomMaterial);
            CoreUtils.Destroy(m_CompositeMaterial);
        }
    }
}