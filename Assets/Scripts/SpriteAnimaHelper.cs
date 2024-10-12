using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[ExecuteAlways]
public class SpriteAnimaHelper : MonoBehaviour
{
    [Tooltip("拖拽SpriteAtlas到这里")] [OnValueChanged(nameof(LoadSprites))]
    public SpriteAtlas spriteAtlas;

    public SpriteRenderer renderer;

    [OnValueChanged(nameof(OnFrameChange))] [PropertyRange(0, "MaxFrameIndex")]
    public int frameIndex = 0;

    [Tooltip("是否按名称排序Sprites")] [OnValueChanged(nameof(LoadSpritesSort))]
    public bool sortSpritesByName = true;

    private int lastFrameIndex;
    [ReadOnly] [SerializeField] private Sprite[] sprites;

    private int MaxFrameIndex => sprites != null ? sprites.Length - 1 : 0;

    private void Awake()
    {
        renderer ??= GetComponent<SpriteRenderer>();
        if (sprites == null) LoadSpritesInternal();
    }

    private void Update()
    {
        // 检查frameIndex是否发生变化（包括由动画系统引起的变化）
        if (frameIndex != lastFrameIndex)
        {
            OnFrameChange(frameIndex);
            lastFrameIndex = frameIndex;
        }
    }

    private void LoadSprites(SpriteAtlas _)
    {
        LoadSpritesInternal();
    }

    private void LoadSpritesSort(bool _)
    {
        LoadSpritesInternal();
    }

    private void LoadSpritesInternal()
    {
        if (spriteAtlas != null)
        {
            Sprite[] loadedSprites = new Sprite[spriteAtlas.spriteCount];
            spriteAtlas.GetSprites(loadedSprites);

            if (sortSpritesByName)
            {
                int KeySelector(Sprite s)
                {
                    var replace = s.name;
                    replace = replace.Replace(spriteAtlas.name, "");
                    replace = replace.Replace("(Clone)", "");
                    replace = replace.Replace("_", "");
                    return int.Parse(replace);
                }

                sprites = loadedSprites.OrderBy(KeySelector).ToArray();
            }
            else
            {
                sprites = loadedSprites;
            }

            Debug.Log($"从SpriteAtlas中加载了 {sprites.Length} 个Sprites。排序: {(sortSpritesByName ? "是" : "否")}");

            // 重置frameIndex以确保在有效范围内
            frameIndex = Mathf.Clamp(frameIndex, 0, MaxFrameIndex);
            OnFrameChange(frameIndex);
        }
        else
        {
            Debug.LogError("请在Inspector中指定一个SpriteAtlas。");
        }
    }

    private void OnFrameChange(int index)
    {
        renderer ??= GetComponent<SpriteRenderer>();
        if (sprites != null && index >= 0 && index < sprites.Length)
        {
            renderer.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("无效的帧索引或Sprites未加载。");
        }
    }

    [Button("重新加载Sprites")]
    private void ReloadSprites()
    {
        LoadSpritesInternal();
    }
}