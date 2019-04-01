using UnityEngine;
using UnityEditor;
using clicker.datatables;
using static clicker.datatables.DataTableItems;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(LocalItemsDataEditor))]
public class TestEditor : Editor
{
    private LocalItemsDataEditor m_Target;
    private ItemFilterTypes m_PrevFilterType = ItemFilterTypes.Food;
    private Dictionary<ItemFilterTypes, Color> m_FilterColors;

    private void OnEnable()
    {
        m_Target = (LocalItemsDataEditor)target;
        m_Target.Data_Items = m_Target.Data_Items.OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Weapons).
                                                  OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Materials).
                                                  OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Ammo).
                                                  OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Food).
                                                  OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Population).ToList();
    }

    void DrawRectButton(ref bool state, string buttonName, Color color)
    {
        Color prevColor = GUI.backgroundColor;
        Rect r = (Rect)EditorGUILayout.BeginVertical("Button");
        GUI.backgroundColor = color;
        if (GUI.Button(r, GUIContent.none))
            state = !state;
        GUILayout.Label(string.Format("{0} ({1})", buttonName, state ? "Hide" : "Show"), EditorStyles.boldLabel);
        GUI.backgroundColor = prevColor;
        EditorGUILayout.EndVertical();
    }

    Color GetColorByItemFilterType(int index, bool reducedAlpha)
    {
        try
        {
            Color result = GetColorForFilter(m_Target.Data_Items[index].FilterTypes[0]);

            if (reducedAlpha)
                result.a *= 0.5f;

            return result;
        }
        catch
        { }

        return Color.white;
    }


    public Color GetColorForFilter(ItemFilterTypes type)
    {
        if (m_FilterColors == null)
        {
            m_FilterColors = new Dictionary<ItemFilterTypes, Color>();

            for (int i = 0; i < m_Target.Color_Filter.Length; i++)
            {
                if (!m_FilterColors.ContainsKey(m_Target.Color_Filter[i].Filter))
                    m_FilterColors.Add(m_Target.Color_Filter[i].Filter, m_Target.Color_Filter[i].Color);
            }
        }


        if (m_FilterColors.ContainsKey(type))
            return m_FilterColors[type];

        return Color.white;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        #region Items
        //ADD ITEM BUTTON
        if (GUILayout.Button("ADD"))
            Debug.Log("ADD ITEM");

        GUILayout.Space(10);

        //ITEMS
        for (int i = 0; i < m_Target.Data_Items.Count; i++)
        {
            if (m_PrevFilterType != m_Target.Data_Items[i].FilterTypes[0])
            {
                if (i != 0)
                {
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                }

                GUI.backgroundColor = GetColorByItemFilterType(i, true);
                GUILayout.BeginVertical("box");

                m_PrevFilterType = m_Target.Data_Items[i].FilterTypes[0];
                GUILayout.Label(m_Target.Data_Items[i].FilterTypes[0].ToString(), EditorStyles.boldLabel);
            }
            
            #region GENERAL
            ////BEGIN - GENERAL
            if (m_Target.Data_Items[i].FoldoutAll)
            {
                GUILayout.Space(10);
                GUI.backgroundColor = GetColorByItemFilterType(i, false);
                GUILayout.BeginVertical("box");
            }

            #region BUTTONS CONTENT - GENERAL
            ////BUTTONS CONTENT - GENERAL
            GUILayout.BeginHorizontal();

            ///Show button
            DrawRectButton(ref m_Target.Data_Items[i].FoldoutAll,
                           m_Target.Data_Items[i].Type.ToString(),
                           m_Target.Data_Items[i].FoldoutAll ? Color.white : GetColorByItemFilterType(i, false));

            ///Remove button
            GUI.backgroundColor = Color.white;
            if (m_Target.Data_Items[i].FoldoutAll && GUILayout.Button("Remove", GUILayout.Width(100)))
                Debug.Log("Remove " + m_Target.Data_Items[i].Type);

            ////END CONTENT BUTTONS
            GUILayout.EndHorizontal();
            #endregion

            #region CONTENT - GENERAL
            ////BEGIN CONTENT - GENERAL
            if (m_Target.Data_Items[i].FoldoutAll)
            {
                GUI.backgroundColor = Color.white;
                GUILayout.BeginVertical("box");

                #region General 
                //General
                m_Target.Data_Items[i].Type = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, m_Target.Data_Items[i].Type);
                m_Target.Data_Items[i].AllowAutocraft = EditorGUILayout.Toggle("AllowAutocraft:", m_Target.Data_Items[i].AllowAutocraft);
                m_Target.Data_Items[i].TickToCreate = EditorGUILayout.IntField("TickToCreate:", m_Target.Data_Items[i].TickToCreate);
                #endregion

                #region Filters
                //Begin Filters
                if (m_Target.Data_Items[i].FoldoutFilters)
                {
                    GUI.backgroundColor = m_Target.Color_Filters;
                    GUILayout.BeginVertical("box");
                }

                #region BUTTONS CONTENT - FILTERS
                ////BUTTONS CONTENT - FILTERS
                GUILayout.BeginHorizontal();

                //Show button
                DrawRectButton(ref m_Target.Data_Items[i].FoldoutFilters,
                               string.Format("{0} ({1})", "FILTERS", m_Target.Data_Items[i].FilterTypes.Count),
                               m_Target.Data_Items[i].FoldoutFilters ? Color.white : m_Target.Color_Filters);

                //Add button
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Add", GUILayout.Width(100)))
                    Debug.Log("Add filter: " + m_Target.Data_Items[i].Type);

                ////END CONTENT BUTTONS
                GUILayout.EndHorizontal();
                #endregion

                #region CONTENT - FILTERS
                //CONTENT - FILTERS
                if (m_Target.Data_Items[i].FoldoutFilters)
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical("box");

                    for (int j = 0; j < m_Target.Data_Items[i].FilterTypes.Count; j++)
                        m_Target.Data_Items[i].FilterTypes[j] = (ItemFilterTypes)EditorGUILayout.EnumPopup(GUIContent.none, m_Target.Data_Items[i].FilterTypes[j]);

                    GUILayout.EndVertical();
                }
                #endregion

                //End Filters
                if (m_Target.Data_Items[i].FoldoutFilters)
                {
                    GUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;
                }
                #endregion

                #region RequiredItems
                //Begin RequiredItems
                if (m_Target.Data_Items[i].FoldoutRequiredItems)
                {
                    GUI.backgroundColor = m_Target.Color_RequiredItems;
                    GUILayout.BeginVertical("box");
                }

                #region BUTTONS CONTENT - REQUIRED ITEMS
                ////BUTTONS CONTENT - REQUIRED ITEMS
                GUILayout.BeginHorizontal();

                //Show button
                DrawRectButton(ref m_Target.Data_Items[i].FoldoutRequiredItems,
                               string.Format("{0} ({1})", "REQUIRED ITEMS", m_Target.Data_Items[i].RequiredItems.Length),
                               m_Target.Data_Items[i].FoldoutRequiredItems ? Color.white : m_Target.Color_RequiredItems);

                //Add button
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Add", GUILayout.Width(100)))
                    Debug.Log("Add required item: " + m_Target.Data_Items[i].Type);

                ////END CONTENT BUTTONS
                GUILayout.EndHorizontal();
                #endregion

                #region CONTENT - REQUIRED ITEMS
                //CONTENT - FILTERS
                if (m_Target.Data_Items[i].FoldoutRequiredItems)
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical("box");

                    for (int j = 0; j < m_Target.Data_Items[i].RequiredItems.Length; j++)
                    {
                        GUILayout.BeginHorizontal();

                        m_Target.Data_Items[i].RequiredItems[j].Type = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, m_Target.Data_Items[i].RequiredItems[j].Type);
                        m_Target.Data_Items[i].RequiredItems[j].Amount = EditorGUILayout.IntField(GUIContent.none, m_Target.Data_Items[i].RequiredItems[j].Amount, GUILayout.Width(50));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }
                #endregion

                //End RequiredItems
                if (m_Target.Data_Items[i].FoldoutRequiredItems)
                {
                    GUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;
                }
                #endregion

                GUILayout.EndVertical();
            }
            ////END CONTENT - GENERAL
            #endregion

            ////END - GENERAL
            if (m_Target.Data_Items[i].FoldoutAll)
            {
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            #endregion
        }

        GUILayout.EndVertical();
        #endregion

        #region CraftIgnoreItems
        GUILayout.Space(20);
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        GUILayout.Label("CRAFT IGNORE ITEMS", EditorStyles.boldLabel);
        if (GUILayout.Button("Add"))
        {
            Debug.Log("Add");
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < m_Target.Data_CraftIgnoreItems.Count; i++)
        {
            GUILayout.BeginVertical("box");

            m_Target.Data_CraftIgnoreItems[i].CraftItem = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, m_Target.Data_CraftIgnoreItems[i].CraftItem);

            if (m_Target.Data_CraftIgnoreItems[i].Foldout)
                GUILayout.BeginVertical("box");

            DrawRectButton(ref m_Target.Data_CraftIgnoreItems[i].Foldout,
                           string.Format("{0} ({1})", "ITEMS", m_Target.Data_CraftIgnoreItems[i].IgnoreItems.Count),
                           m_Target.Data_CraftIgnoreItems[i].Foldout ? m_Target.Color_SelectedCraftIgnoreItem : Color.white);

            if (m_Target.Data_CraftIgnoreItems[i].Foldout)
            {
                if (GUILayout.Button("Add"))
                {
                    Debug.Log("Add");
                }

                for (int j = 0; j < m_Target.Data_CraftIgnoreItems[i].IgnoreItems.Count; j++)
                {
                    GUILayout.BeginHorizontal();
                    m_Target.Data_CraftIgnoreItems[i].IgnoreItems[j] = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, m_Target.Data_CraftIgnoreItems[i].IgnoreItems[j]);
                    if (GUILayout.Button("Remove"))
                    {
                        Debug.Log("Remove " + m_Target.Data_CraftIgnoreItems[i].IgnoreItems[j]);
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        #endregion
    }
}