using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Laba2NYSS.Models
{
    class ThreatUpdate
    {
        public string Id { get; private set; }
        public string FieldName { get; private set; }
        public string PrevValue { get; private set; }
        public string NewValue { get; private set; }

        public ThreatUpdate(string id, string fieldName, string prevValue, string newValue)
        {
            Id = id;
            FieldName = fieldName;
            PrevValue = prevValue;
            NewValue = newValue;
        }

        public static List<ThreatUpdate> CompareThreats(Threat oldThreat, Threat newThreat)
        {
            var updates = new List<ThreatUpdate>();

            if (oldThreat.Name != newThreat.Name)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Наименование", oldThreat.Name, newThreat.Name));
            }
            if (oldThreat.Description != newThreat.Description)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Описание", oldThreat.Description, newThreat.Description));
            }
            if (oldThreat.Source != newThreat.Source)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Источник", oldThreat.Source, newThreat.Source));
            }
            if (oldThreat.Object != newThreat.Object)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Объект воздействия", oldThreat.Object, newThreat.Object));
            }
            if (oldThreat.PrivacyViolation != newThreat.PrivacyViolation)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Нарушение конфиденциальности", oldThreat.PrivacyViolation, newThreat.PrivacyViolation));
            }
            if (oldThreat.IntegrityViolation != newThreat.IntegrityViolation)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Нарушение целостности", oldThreat.IntegrityViolation, newThreat.IntegrityViolation));
            }
            if (oldThreat.AccessViolation != newThreat.AccessViolation)
            {
                updates.Add(new ThreatUpdate(oldThreat.Id, "Нарушение доступности", oldThreat.AccessViolation, newThreat.AccessViolation));
            }

            return updates;
        }
    }
}
