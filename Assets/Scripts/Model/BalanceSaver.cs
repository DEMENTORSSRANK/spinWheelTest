using UnityEngine;

namespace Model
{
    public class BalanceSaver
    {
        private const string BalanceKey = "Balance";
        
        public void Save(int balance)
        {
            PlayerPrefs.SetInt(BalanceKey, balance);
        }

        public bool TryGetSaved(out int balance)
        {
            bool success = PlayerPrefs.HasKey(BalanceKey); 
            
            balance = success ? PlayerPrefs.GetInt(BalanceKey) : 0;

            return success;
        }
    }
}