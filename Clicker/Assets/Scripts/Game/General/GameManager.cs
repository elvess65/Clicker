using clicker.datatables;
using UnityEngine;

namespace clicker.general
{
    public class GameManager : MonoBehaviour
    {   
        void Start()
        {
            DataTableItems.SetData(GetComponent<LocalItemsDataEditor>().Data_Items);
            account.Account acc = new account.Account();    
        }
    }
}
