using UnityEngine;
using System.Collections;
    [System.Serializable]
    public class Quest
    {
        public enum QuestProgress { notAvailable, available, accepted, complete, done }
        public string title;
        public int id;
        public QuestProgress progress;
        public string description;
        public string hint;
        public string congratulation;
        public string summery;
        public int nextQuest;

        public Item questObjective;
        public int questObjectiveCount;
        public int questObjectiveRequirement;

        public int goldReward;
        public Item itemReward;
        /*public Quest()
        {

        }

        private List<IQuestInformation> objectives;
        private IQuestInformation information;
        public IQuestInformation Information
        {
            get { return information; }
        }*/
    }


