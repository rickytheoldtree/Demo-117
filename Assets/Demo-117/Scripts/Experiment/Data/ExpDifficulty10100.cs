using Newtonsoft.Json;
using RicKit.Experiment;
using RicKit.RFramework;
using UnityEngine;

namespace Demo_117.Experiment.Data
{
    public class ExpDifficulty10100 : BaseExperiment, ICanGetLocator<Entity>
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override BaseExperiment FromString(string data)
        {
            return JsonConvert.DeserializeObject<ExpDifficulty10100>(data);
        }

        public override void SetGroup()
        {
#if UNITY_IOS
            group = ExperimentGroup.None;
            return;
#endif
            var country = /*PlatformUtils.GetCountry().ToLower().Trim();*/ "en";
            if (country == "br" || country == "bra" || Application.isEditor)
            {
                group = Random.value < 0.5f ? ExperimentGroup.A : ExperimentGroup.S;
                //发送分组事件，参数为分组结果
                /*this.GetService<IFirebaseService>().LogEvent(() => "exp_difficulty_10100_group", 
                    new FirebaseParameter(() => "group", group.ToString()));*/
            }
            else
            {
                group = ExperimentGroup.None;
            }
        }

        public override ExperimentTargetUser TargetUser => ExperimentTargetUser.NewUser;
    }
}