using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic.Minigame;
using FishNet.Connection;
using FishNet.Object;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ShowQuestRecords : NetworkBehaviour
    {
        public TextMeshProUGUI Text;
        public QuestStaticData[] QuestStaticData;

        private Dictionary<int, float> _records = new Dictionary<int, float>();

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            MinigameManagerSinglton.Instance.QuestRecordUpdated += InstanceOnQuestRecordUpdated;
            UpdateText();
            MinigameManagerSinglton.Instance.RequestRecordsUpdateServerRPC();
            
        }

        private void OnDestroy()
        {
            MinigameManagerSinglton.Instance.QuestRecordUpdated -= InstanceOnQuestRecordUpdated;
        }

        private void InstanceOnQuestRecordUpdated(int arg1, float record)
        {
            _records[arg1] = record;
            UpdateText();
        }

        private void UpdateText()
        {
            string txt = "Лучшее время:\n";
            foreach (var data in QuestStaticData)
            {
                if (!_records.ContainsKey(data.Id))
                {
                    txt = txt + " " + data.Destination + ":" + " " + "-" + "\n";
                }
                else
                {
                    txt = txt + " " + data.Destination + ":" + " " + Mathf.Round(_records[data.Id]) + " c." + "\n";
                }
            }

            Text.text = txt;
        }
    }
}