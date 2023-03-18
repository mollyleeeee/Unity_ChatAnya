using UnityEngine.UIElements;

namespace UnityEditor.Tilemaps
{
    internal class GridPaintPaletteWindowSplitView : VisualElement
    {
        private static readonly string ussClassName = "unity-tilepalette-splitview";
        private static readonly string brushesUssClassName = "unity-tilepalette-splitview-brushes";
        private static readonly string splitViewDataKey = "unity-tilepalette-splitview-data";

        private TwoPaneSplitView m_SplitView;
        private TilePaletteElement m_PaletteElement;
        private TilePaletteBrushElementToggle m_BrushElementToggle;
        private float m_LastSplitDimension = 200f;

        public TilePaletteElement paletteElement => m_PaletteElement;

        public bool isVerticalOrientation
        {
            get
            {
                return m_SplitView.orientation == TwoPaneSplitViewOrientation.Vertical;
            }
            set
            {
                m_SplitView.orientation =
                    value ? TwoPaneSplitViewOrientation.Vertical : TwoPaneSplitViewOrientation.Horizontal;
            }
        }

        private float fullLength => isVerticalOrientation ? layout.height : layout.width;

        private bool isMinSplit => (fullLength - m_SplitView.fixedPaneDimension) <= minSplitDimension;

        private float minSplitDimension => isVerticalOrientation ? 24f : 0f;

        public void ChangeSplitDimensions(float dimension)
        {
            var newLength = fullLength - dimension;
            var diff = newLength - m_SplitView.fixedPaneDimension;
            if (m_SplitView.m_Resizer != null)
                m_SplitView.m_Resizer.ApplyDelta(diff);
        }

        public GridPaintPaletteWindowSplitView(EditorWindow editorWindow, bool isVerticalOrientation)
        {
            AddToClassList(ussClassName);

            name = "tilePaletteSplitView";
            TilePaletteOverlayUtility.SetStyleSheet(this);

            m_PaletteElement = new TilePaletteElement();

            var brushesElement = new VisualElement();
            brushesElement.AddToClassList(brushesUssClassName);
            brushesElement.Add(new TilePaletteBrushesPopup());
            brushesElement.Add(new TilePaletteBrushInspectorElement());

            m_SplitView = new TwoPaneSplitView(0, 0, isVerticalOrientation ? TwoPaneSplitViewOrientation.Vertical : TwoPaneSplitViewOrientation.Horizontal);
            m_SplitView.contentContainer.Add(m_PaletteElement);
            m_SplitView.contentContainer.Add(brushesElement);
            Add(m_SplitView);

            m_SplitView.viewDataKey = splitViewDataKey;

            brushesElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            m_BrushElementToggle = this.Q<TilePaletteBrushElementToggle>();
            m_BrushElementToggle.ToggleChanged += BrushElementToggleChanged;
            m_BrushElementToggle.SetValueWithoutNotify(!isMinSplit);
        }

        private void BrushElementToggleChanged(bool show)
        {
            ChangeSplitDimensions(show ? m_LastSplitDimension : minSplitDimension);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            m_BrushElementToggle.SetValueWithoutNotify(!isMinSplit);

            var newDimension = fullLength - m_SplitView.fixedPaneDimension;
            if (newDimension > minSplitDimension)
                m_LastSplitDimension = newDimension;
        }
    }
}
