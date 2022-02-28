namespace DigitalWorld.Logic
{
    public partial class Trigger
    {
        #region Event
        public void DispatchEvent(IEvent ev)
        {
            if (ev.Id == this.ListenEventId)
            {
                this.Process(ev);
            }
        }

        private void Process(IEvent ev)
        {
            this.triggeringEvent = ev;

            bool conf = false;
            ConstructTriggerUnit(ev);
            if (CheckLogic == ECheckLogic.And)
            {
                conf = true;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    conf = conf && conditions[i].Check();
                    if (!conf) break;
                }
            }
            else
            {
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (!conditions[i].Enabled) continue;
                    conf = conf || conditions[i].Check();
                    if (conf) break;
                }
            }

            if (conf)
            {
                Invoke();
            }
        }
        #endregion

        #region Logic
        public virtual void OnUpdate(int delta)
        {
            if (this.enabled)
            {
                this.runningTime += delta;
            }
        }

        private void ConstructTriggerUnit(IEvent ev)
        {
            //switch ((EEve)ev.Id)
            //{
            //    case EventEnum.GameStarted:
            //        break;
            //    case EventEnum.BuildingDeath:
            //        triggeringUnit.id = ((BuildingDeathEvent)ev).buildingId;
            //        triggeringUnit.identity = (uint)((BuildingDeathEvent)ev).buildingId;
            //        break;
            //    case EventEnum.UnitDeath:
            //        triggeringUnit.id = ((UnitDeathEvent)ev).unitId;
            //        triggeringUnit.identity = (uint)(((UnitDeathEvent)ev).unitId);
            //        break;
            //    case EventEnum.UnitEntered:
            //        break;
            //    case EventEnum.UnitBorn:
            //        triggeringUnit.id = ((UnitBornEvent)ev).unitId;
            //        triggeringUnit.identity = (uint)((UnitBornEvent)ev).identity;
            //        break;
            //}
        }

        private void Invoke()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].Enabled) continue;
                actions[i].Invoke();
            }
        }
        #endregion
    }
}
