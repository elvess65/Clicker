using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Test.data;
using clicker.datatables;
using static clicker.datatables.DataTableItems;
using System;
using System.Linq;

[CustomEditor(typeof(LocalItemsDataEditor))]
public class TestEditor : Editor
{
    LocalItemsDataEditor t;
    ItemsData[] items;

    private void OnEnable()
    {
        Debug.Log("1");
        t = (LocalItemsDataEditor)target;
        items = new ItemsData[t.Data_Items.Length];
        for (int i = 0; i < t.Data_Items.Length; i++)
        {
            items[i] = t.Data_Items[i];
        }
        items = items.OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Weapons).
                      OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Materials).
                      OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Ammo).
                      OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Food).
                      OrderBy(i => i.FilterTypes[0] == ItemFilterTypes.Population).ToArray();
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
    ItemFilterTypes prevFilterType = ItemFilterTypes.Food;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("ADD"))
        {
            Debug.Log("ADD ITEM");
        }
        
        GUILayout.Space(10);



        for (int i = 0; i < items.Length; i++)
        {
            if (prevFilterType != items[i].FilterTypes[0])
            {
                prevFilterType = items[i].FilterTypes[0];
                GUILayout.Label(items[i].FilterTypes[0].ToString());
            }
            #region GENERAL
            ////BEGIN - GENERAL
            if (t.Data_Items[i].FoldoutAll)
            {
                GUILayout.Space(10);
                GUI.backgroundColor = t.Color_General;
                GUILayout.BeginVertical("box");
            }

            #region BUTTONS CONTENT - GENERAL
            ////BUTTONS CONTENT - GENERAL
            GUILayout.BeginHorizontal();

            ///Show button
            DrawRectButton(ref t.Data_Items[i].FoldoutAll,
                           items[i].Type.ToString(), 
                           t.Data_Items[i].FoldoutAll ? Color.white : t.Color_General);

            ///Remove button
            GUI.backgroundColor = Color.white;
            if (t.Data_Items[i].FoldoutAll && GUILayout.Button("Remove", GUILayout.Width(100)))
                Debug.Log("Remove " + t.Data_Items[i].Type);

            ////END CONTENT BUTTONS
            GUILayout.EndHorizontal();
            #endregion

            #region CONTENT - GENERAL
            ////BEGIN CONTENT - GENERAL
            if (t.Data_Items[i].FoldoutAll)
            {
                GUI.backgroundColor = Color.white;
                GUILayout.BeginVertical("box");

                #region General 
                //General
                t.Data_Items[i].Type = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, t.Data_Items[i].Type);
                t.Data_Items[i].AllowAutocraft = EditorGUILayout.Toggle("AllowAutocraft:", t.Data_Items[i].AllowAutocraft);
                t.Data_Items[i].TickToCreate = EditorGUILayout.IntField("TickToCreate:", t.Data_Items[i].TickToCreate);
                #endregion

                #region Filters
                //Begin Filters
                if (t.Data_Items[i].FoldoutFilters)
                {
                    GUI.backgroundColor = t.Color_Filters;
                    GUILayout.BeginVertical("box");
                }

                #region BUTTONS CONTENT - FILTERS
                ////BUTTONS CONTENT - FILTERS
                GUILayout.BeginHorizontal();

                //Show button
                DrawRectButton(ref t.Data_Items[i].FoldoutFilters, 
                               string.Format("{0} ({1})", "FILTERS", t.Data_Items[i].FilterTypes.Count),
                               t.Data_Items[i].FoldoutFilters ? Color.white : t.Color_Filters);

                //Add button
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Add", GUILayout.Width(100)))
                    Debug.Log("Add filter: " + t.Data_Items[i].Type);

                ////END CONTENT BUTTONS
                GUILayout.EndHorizontal();
                #endregion

                #region CONTENT - FILTERS
                //CONTENT - FILTERS
                if (t.Data_Items[i].FoldoutFilters)
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical("box");

                    for (int j = 0; j < t.Data_Items[i].FilterTypes.Count; j++)
                        t.Data_Items[i].FilterTypes[j] = (ItemFilterTypes)EditorGUILayout.EnumPopup(GUIContent.none, t.Data_Items[i].FilterTypes[j]);

                    GUILayout.EndVertical();
                }
                #endregion

                //End Filters
                if (t.Data_Items[i].FoldoutFilters)
                {
                    GUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;
                }
                #endregion

                #region RequiredItems
                //Begin RequiredItems
                if (t.Data_Items[i].FoldoutRequiredItems)
                {
                    GUI.backgroundColor = t.Color_RequiredItems;
                    GUILayout.BeginVertical("box");
                }
                    
                #region BUTTONS CONTENT - REQUIRED ITEMS
                ////BUTTONS CONTENT - REQUIRED ITEMS
                GUILayout.BeginHorizontal();

                //Show button
                DrawRectButton(ref t.Data_Items[i].FoldoutRequiredItems, 
                               string.Format("{0} ({1})", "REQUIRED ITEMS", t.Data_Items[i].RequiredItems.Length), 
                               t.Data_Items[i].FoldoutRequiredItems ? Color.white : t.Color_RequiredItems);

                //Add button
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Add", GUILayout.Width(100)))
                    Debug.Log("Add required item: " + t.Data_Items[i].Type);

                ////END CONTENT BUTTONS
                GUILayout.EndHorizontal();
                #endregion

                #region CONTENT - REQUIRED ITEMS
                //CONTENT - FILTERS
                if (t.Data_Items[i].FoldoutRequiredItems)
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = Color.white;
                    GUILayout.BeginVertical("box");

                    for (int j = 0; j < t.Data_Items[i].RequiredItems.Length; j++)
                    {
                        GUILayout.BeginHorizontal();

                        t.Data_Items[i].RequiredItems[j].Type = (ItemTypes)EditorGUILayout.EnumPopup(GUIContent.none, t.Data_Items[i].RequiredItems[j].Type);
                        t.Data_Items[i].RequiredItems[j].Amount = EditorGUILayout.IntField(GUIContent.none, t.Data_Items[i].RequiredItems[j].Amount, GUILayout.Width(50));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                }
                #endregion

                //End RequiredItems
                if (t.Data_Items[i].FoldoutRequiredItems)
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
            if (t.Data_Items[i].FoldoutAll)
            {
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
            #endregion
        }
    }
}