using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;


namespace Nexomon1Model
{
    public static class Consts
    {
        private static Dictionary<string, Bitmap>? itemspic;
        public static Dictionary<string, Bitmap> ItemsPic 
        {
            get
            {
                if(itemspic==null)
                {
                    var folderUri = new Uri($"avares://NEXHEX/Nex1Assets/items-icons");
                    var assetUris = AssetLoader.GetAssets(folderUri, null);
                    itemspic = new Dictionary<string, Bitmap>();
                    foreach (var uri in assetUris)
                    {
                        string name = uri.AbsolutePath.TrimStart('/');
                        name = Path.GetFileNameWithoutExtension(name);
                        var uriimg = new Uri($"avares://NEXHEX/Nex1Assets/items-icons/{name}.png");
                        using var stream = AssetLoader.Open(uriimg);
                        Bitmap img = new Bitmap(stream);
                        itemspic.Add(name, img);
                    }
                }
                return itemspic;
            }
        }
        private static Dictionary<string, Bitmap>? monsterspic;
        public static Dictionary<string, Bitmap> MonstersPic 
        {
            get
            {
                if (monsterspic == null)
                {
                    //loading MonstersPic
                    var folderUri = new Uri($"avares://NEXHEX/Nex1Assets/monsters-icons");
                    var assetUris = AssetLoader.GetAssets(folderUri, null);
                    monsterspic = new Dictionary<string, Bitmap>();
                    foreach (var uri in assetUris)
                    {
                        string name = uri.AbsolutePath.TrimStart('/');
                        name = Path.GetFileNameWithoutExtension(name);
                        var uriimg = new Uri($"avares://NEXHEX/Nex1Assets/monsters-icons/{name}.png");
                        using var stream = AssetLoader.Open(uriimg);
                        Bitmap img = new Bitmap(stream);
                        monsterspic.Add(name, img);
                    }
                }
                return monsterspic;
            }
        }
        private static Dictionary<int, Bitmap>? skillspic;
        public static Dictionary<int, Bitmap> SkillsPic 
        {
            get
            {
                if (skillspic == null)
                {
                    var folderUri = new Uri($"avares://NEXHEX/Nex1Assets/skill-name-icons");
                    var assetUris = AssetLoader.GetAssets(folderUri, null);
                    skillspic = new Dictionary<int, Bitmap>();
                    foreach (var uri in assetUris)
                    {
                        string name = uri.AbsolutePath.TrimStart('/');
                        name = Path.GetFileNameWithoutExtension(name);
                        var uriimg = new Uri($"avares://NEXHEX/Nex1Assets/skill-name-icons/{name}/{name}.png");
                        using var stream = AssetLoader.Open(uriimg);
                        Bitmap img = new Bitmap(stream);
                        if (!skillspic.ContainsKey(int.Parse(name)))
                            skillspic.Add(int.Parse(name), img);
                    }

                }
                skillspic = skillspic.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                return skillspic;
            }
        }
        private static Dictionary<int, string>? skillsname;
        public static Dictionary<int, string> SkillsName 
        {
            get
            {
                if (skillsname == null)
                {
                    //loading SkillsPic & SkillsName
                    var folderUri = new Uri($"avares://NEXHEX/Nex1Assets/skill-name-icons");
                    var assetUris = AssetLoader.GetAssets(folderUri, null);
                    skillsname = new Dictionary<int, string>();
                    foreach (var uri in assetUris)
                    {
                        string name = uri.AbsolutePath.TrimStart('/');
                        name = Path.GetFileNameWithoutExtension(name);
                        var uriname = new Uri($"avares://NEXHEX/Nex1Assets/skill-name-icons/{name}/{name}.txt");
                        string skillname = (new StreamReader(AssetLoader.Open(uriname))).ReadLine()!;
                        if(!skillsname.ContainsKey(int.Parse(name)))
                        skillsname.Add(int.Parse(name), skillname);
                    }
                }
                skillsname = skillsname.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                return skillsname;
            } 
        }
        private static ObservableCollection<Skill>? skills;
        public static ObservableCollection<Skill> Skills 
        {
            get
            {
                if (skills == null)
                {
                    skills = new ObservableCollection<Skill>();
                    skills.Add(new Skill(0));
                    foreach (int skillid in SkillsName.Keys)
                    {
                        skills.Add(new Skill(skillid));
                    }
                }
                return skills;
            } 
        }
        private static Bitmap? defaultbitmap;
        public static Bitmap DefaultBitmap 
        {
            get
            {
                if (defaultbitmap == null)
                {
                    defaultbitmap = new WriteableBitmap(new PixelSize(1, 1), new Vector(96, 96));
                }
                return defaultbitmap;
            } 
        }
        private static ObservableCollection<string>? monstersnames;
        public static ObservableCollection<string> MonstersNames 
        {
            get
            {
                if (monstersnames == null)
                {
                    monstersnames = new ObservableCollection<string>(MonstersPic.Keys);
                }
                return monstersnames;
            } 
        }
        public static Dictionary<string, Dictionary<string, float>> MonsterGrowthData { get; set; } = new Dictionary<string, Dictionary<string, float>>
        {
            ["ABADDOG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 81.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["ACCA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["ALGAMAID"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 53.0f,
                ["atkGrowth"] = 92.0f,
                ["defGrowth"] = 47.0f,
                ["speedGrowth"] = 85.0f,
            },
            ["ALPANIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 96.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["ALPOCA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 96.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["AMPHANT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 119.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 91.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["ANGALO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 97.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["ANGELIQUE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 97.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["ARCTIVORE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 98.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["ARIGAMI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 55.0f,
                ["atkGrowth"] = 91.0f,
                ["defGrowth"] = 48.0f,
                ["speedGrowth"] = 84.0f,
            },
            ["ARISTOKAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["ARNUT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 109.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["ARQUA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 123.0f,
                ["atkGrowth"] = 137.0f,
                ["defGrowth"] = 93.0f,
                ["speedGrowth"] = 50.0f,
            },
            ["ASTEOR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["AULION"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 92.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["AULOT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 92.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["AZITE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 96.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["AZITON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 96.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["BALLERY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["BARKRUNCH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 81.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["BARKY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 81.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["BARVER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["BASTEN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["BEDAM"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["BERAWN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 114.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["BEVY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["BILABEKA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["BLIZBROW"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 118.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["BLIZLOO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 118.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["BLIZO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 118.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["BLURK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 76.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["BOGARP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["BOLTUSK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 119.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 91.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["BOSDOUR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 76.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["BOVICUE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["BOWISH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 63.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 53.0f,
                ["speedGrowth"] = 80.0f,
            },
            ["BOXROCK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 85.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["BOZOFIN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 91.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 72.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["BREESHY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 78.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["BRODUO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 116.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["BROSHIELD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 116.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["BROWI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 116.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["BULIE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["BULKEN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 56.0f,
                ["atkGrowth"] = 97.0f,
                ["defGrowth"] = 49.0f,
                ["speedGrowth"] = 83.0f,
            },
            ["BUNROCK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 85.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["CACTEONE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 86.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["CACTINO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 86.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["CAMOLEON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 53.0f,
                ["atkGrowth"] = 92.0f,
                ["defGrowth"] = 47.0f,
                ["speedGrowth"] = 85.0f,
            },
            ["CANDRAMA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 50.0f,
                ["atkGrowth"] = 90.0f,
                ["defGrowth"] = 45.0f,
                ["speedGrowth"] = 86.0f,
            },
            ["CARNAGRIUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["CAVEL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["CHAROON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 89.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 71.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["CHEEKMUNCH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["CHEEKMUNK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["CHICOLO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["CHIRPOINT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 75.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["CLOBOLT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["CLONKEY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["CLUNDER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["COLUMBRA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 100.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["CORDYANT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 72.0f,
                ["atkGrowth"] = 106.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["COROZARD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["COTONDEE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["CRAGODILLO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 117.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["CRONIC"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 71.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["DANDELLOON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 70.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["DANDEMILL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 70.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["DARINE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 135f,
                ["atkGrowth"] = 151f,
                ["defGrowth"] = 102f,
                ["speedGrowth"] = 50.0f,
            },
            ["DETHWURM"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 108.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["DIGGA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 64.0f,
                ["atkGrowth"] = 95.0f,
                ["defGrowth"] = 54.0f,
                ["speedGrowth"] = 79.0f,
            },
            ["DRACLONE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 108.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["DRAKUZA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 91.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 72.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["DRASH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 105.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["DRIFTLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["DROMERUF"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["DUNEFIN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 102.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["DURTWORM"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 108.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["EELE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 91.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 72.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["EELOWATT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 91.0f,
                ["atkGrowth"] = 125.0f,
                ["defGrowth"] = 72.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["EMOTEL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 62.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 53.0f,
                ["speedGrowth"] = 80.0f,
            },
            ["EMPERABEE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 72.0f,
                ["atkGrowth"] = 105.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["ENERFOX"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 62.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 53.0f,
                ["speedGrowth"] = 80.0f,
            },
            ["ENERMINE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 92.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["EXCAVAT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 65.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 55.0f,
                ["speedGrowth"] = 79.0f,
            },
            ["EXPUNK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["FANFROU"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 57.0f,
                ["atkGrowth"] = 96.0f,
                ["defGrowth"] = 50.0f,
                ["speedGrowth"] = 83.0f,
            },
            ["FAVOLIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["FELICIENT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["FELYNTH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["FERODILE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["FEROSERA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["FETHRA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 108.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["FETIDOOM"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["FIRIUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["FLAMINGLO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 82.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["FLANDLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 70.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["FLARINGO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 82.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["FLEECIUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["FLEXEL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 89.0f,
                ["atkGrowth"] = 117.0f,
                ["defGrowth"] = 71.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["FLOATLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["FLORANTIS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 52.0f,
                ["atkGrowth"] = 93.0f,
                ["defGrowth"] = 46.0f,
                ["speedGrowth"] = 85.0f,
            },
            ["FLOREUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 82.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["FLOUSPER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 82.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["FLUZARD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["FLYBUSTER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 51.0f,
                ["atkGrowth"] = 93.0f,
                ["defGrowth"] = 46.0f,
                ["speedGrowth"] = 86.0f,
            },
            ["FONA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 121.0f,
                ["atkGrowth"] = 137.0f,
                ["defGrowth"] = 92.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["FOWLING"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 77.0f,
                ["atkGrowth"] = 100.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["FRANCELSTUD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 99.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["FRILIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["FROGUE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 97.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["FROPIP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 51.0f,
                ["atkGrowth"] = 91.0f,
                ["defGrowth"] = 45.0f,
                ["speedGrowth"] = 86.0f,
            },
            ["FROPPE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["FRULF"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 98.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["FUMOUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["FURPY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 116.0f,
                ["atkGrowth"] = 103.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["FUZZTINO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 61.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 52.0f,
                ["speedGrowth"] = 81.0f,
            },
            ["GALVARIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["GEMEEN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 69.0f,
                ["atkGrowth"] = 103.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["GEMIR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 69.0f,
                ["atkGrowth"] = 103.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["GERVORE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 79.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["GNALIGATOR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["GNOB"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 64.0f,
                ["atkGrowth"] = 96.0f,
                ["defGrowth"] = 54.0f,
                ["speedGrowth"] = 79.0f,
            },
            ["GOLEMATON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 90.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 72.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["GORGONITA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 68.0f,
                ["atkGrowth"] = 105.0f,
                ["defGrowth"] = 57.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["GRITCH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["GRUMUG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 80.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["GRUNDA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 123.0f,
                ["atkGrowth"] = 138.0f,
                ["defGrowth"] = 94.0f,
                ["speedGrowth"] = 50.0f,
            },
            ["GRUSH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 118.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["HAMOO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["HEIFRY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["HEXIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 134f,
                ["atkGrowth"] = 161f,
                ["defGrowth"] = 102f,
                ["speedGrowth"] = 50.0f,
            },
            ["HEXOPUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 109.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["HISSA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 100.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["HOARDENT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["HOBYN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 50.0f,
                ["atkGrowth"] = 91.0f,
                ["defGrowth"] = 45.0f,
                ["speedGrowth"] = 86.0f,
            },
            ["HOLLOWRETCH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 90.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 71.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["HONBUZZ"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 72.0f,
                ["atkGrowth"] = 105.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["HOPPETI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 85.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["HURRISTAG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["HYDRAX"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["IDOLETTE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["IGNEAMP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 74.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["IGNIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 63.0f,
                ["atkGrowth"] = 95.0f,
                ["defGrowth"] = 54.0f,
                ["speedGrowth"] = 80.0f,
            },
            ["JAGSTRICH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["JAMPA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["JEETA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["JELLIEN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 78.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["JELLUFO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 78.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["KIKLICK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 52.0f,
                ["atkGrowth"] = 92.0f,
                ["defGrowth"] = 46.0f,
                ["speedGrowth"] = 85.0f,
            },
            ["KINDALA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 99.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["KINDEOR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["KITSUNOX"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 88.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["KROMATICE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["KROWR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 134f,
                ["atkGrowth"] = 156f,
                ["defGrowth"] = 102f,
                ["speedGrowth"] = 60.0f,
            },
            ["KUMACHO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 114.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["KYTE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 98.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["LAGOROVE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 85.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["LANTORA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 58.0f,
                ["atkGrowth"] = 100.0f,
                ["defGrowth"] = 50.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["LAVAMP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 74.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["LEAFEGG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 82.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["LUBERG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 98.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["LUHAVA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 131f,
                ["atkGrowth"] = 141f,
                ["defGrowth"] = 105f,
                ["speedGrowth"] = 51.0f,
            },
            ["LUXA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 122.0f,
                ["atkGrowth"] = 138.0f,
                ["defGrowth"] = 93.0f,
                ["speedGrowth"] = 50.0f,
            },
            ["LYCOPAIN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["MAGNEGO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["MAGNYMPH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["MAGONFLY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["MAGPILLAR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 100.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["MAGPOT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 100.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["MALFUNCTOR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["MANDRASS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["MANDROVE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["MANZAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["MELLOW"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 75.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["METTA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 121.0f,
                ["atkGrowth"] = 138.0f,
                ["defGrowth"] = 92.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["MIGLET"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 81.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["MISTRALION"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["MOINK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 81.0f,
                ["atkGrowth"] = 107.0f,
                ["defGrowth"] = 66.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["MOLOBOMB"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 65.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 55.0f,
                ["speedGrowth"] = 79.0f,
            },
            ["MONEXUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["MONKAPOW"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 118.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["MONOLIX"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["MOUNDRIAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 113.0f,
                ["atkGrowth"] = 118.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["MUDLUG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 80.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["MUSHAMUSHA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 86.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["MUSTINO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 61.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 52.0f,
                ["speedGrowth"] = 81.0f,
            },
            ["NARA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 120.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 91.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["NEKOMBA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 88.0f,
                ["atkGrowth"] = 117.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["NIGHTITE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 69.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 57.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["NIMBURST"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 93.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["NOCGOYLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 69.0f,
                ["atkGrowth"] = 112.0f,
                ["defGrowth"] = 57.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["NUTCHOK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 109.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["OBELINK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["OGOON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 139f,
                ["atkGrowth"] = 151f,
                ["defGrowth"] = 101f,
                ["speedGrowth"] = 62.0f,
            },
            ["OMNICRON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 120.0f,
                ["atkGrowth"] = 139.0f,
                ["defGrowth"] = 92.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["ONSLOCEROS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 119.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 91.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["OSTRIDGE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["OWLIE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["PANDICUB"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 56.0f,
                ["atkGrowth"] = 97.0f,
                ["defGrowth"] = 49.0f,
                ["speedGrowth"] = 83.0f,
            },
            ["PARCHYON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 89.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 71.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["PARRPY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 66.0f,
                ["atkGrowth"] = 95.0f,
                ["defGrowth"] = 55.0f,
                ["speedGrowth"] = 78.0f,
            },
            ["PENTAPUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 109.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 84.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["PERCHARA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["PERLEO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 66.0f,
                ["atkGrowth"] = 90.0f,
                ["defGrowth"] = 56.0f,
                ["speedGrowth"] = 78.0f,
            },
            ["PETRIL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 106.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["PHARAMOTH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 67.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 56.0f,
                ["speedGrowth"] = 78.0f,
            },
            ["PINCER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 80.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["PINCERTRON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 80.0f,
                ["atkGrowth"] = 101.0f,
                ["defGrowth"] = 65.0f,
                ["speedGrowth"] = 71.0f,
            },
            ["PIRA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 67.0f,
                ["atkGrowth"] = 100.0f,
                ["defGrowth"] = 56.0f,
                ["speedGrowth"] = 78.0f,
            },
            ["PLATYFOO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 59.0f,
                ["atkGrowth"] = 94.0f,
                ["defGrowth"] = 51.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["POLENBLOM"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 58.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 50.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["POLERINA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 58.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 50.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["POMPATIEL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 76.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["PONIRIUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["PORAPORA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 86.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 69.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["PRISMAZOR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["RAAMU"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["RAFODILLO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 117.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["RATAPON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 77.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["RATATUSK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 77.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["REPLICANTI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 57.0f,
                ["atkGrowth"] = 96.0f,
                ["defGrowth"] = 49.0f,
                ["speedGrowth"] = 83.0f,
            },
            ["RESONECT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 71.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["ROBATT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 99.0f,
                ["atkGrowth"] = 129.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["ROCSTRICH"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["ROOHOPU"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["ROOSKIPA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 112.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 55.0f,
            },
            ["SATRAY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 98.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["SAXIA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 60.0f,
                ["atkGrowth"] = 106.0f,
                ["defGrowth"] = 51.0f,
                ["speedGrowth"] = 81.0f,
            },
            ["SCABRILLO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 117.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["SCIZARINA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 79.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["SCURREL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 61.0f,
                ["atkGrowth"] = 94.0f,
                ["defGrowth"] = 52.0f,
                ["speedGrowth"] = 81.0f,
            },
            ["SEADIAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["SEAGUARD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["SEAPHON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 83.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 70.0f,
            },
            ["SEATAIL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 102.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["SEATUSK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 102.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["SEEDREAD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 90.0f,
                ["atkGrowth"] = 115.0f,
                ["defGrowth"] = 71.0f,
                ["speedGrowth"] = 66.0f,
            },
            ["SERAFORONA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 97.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["SERALA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 99.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 77.0f,
                ["speedGrowth"] = 62.0f,
            },
            ["SHARNOLL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 102.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["SHASHOCK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 65.0f,
                ["atkGrowth"] = 103.0f,
                ["defGrowth"] = 55.0f,
                ["speedGrowth"] = 79.0f,
            },
            ["SHERAL"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 105.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["SHEREEF"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 105.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["SHERMIT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 105.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 82.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["SHREPIAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 111.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 86.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["SHRUBBIT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 85.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["SILKMAID"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 71.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["SILKPHA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 67.0f,
                ["atkGrowth"] = 98.0f,
                ["defGrowth"] = 56.0f,
                ["speedGrowth"] = 78.0f,
            },
            ["SKOI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 78.0f,
                ["atkGrowth"] = 108.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["SLIFFERY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["SLITHERA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 100.0f,
                ["atkGrowth"] = 131.0f,
                ["defGrowth"] = 78.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["SONICOCO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 71.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["SPECTRICE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["SPINA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 55.0f,
                ["atkGrowth"] = 94.0f,
                ["defGrowth"] = 48.0f,
                ["speedGrowth"] = 84.0f,
            },
            ["SPINK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["SPROUTI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 58.0f,
                ["atkGrowth"] = 100.0f,
                ["defGrowth"] = 50.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["SPRUNK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 132.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["SPURK"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 76.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["STEAMOUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["STELMO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 94.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 74.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["STOAIC"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 92.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["STOOGU"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 54.0f,
                ["atkGrowth"] = 90.0f,
                ["defGrowth"] = 48.0f,
                ["speedGrowth"] = 84.0f,
            },
            ["SUAVIAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 77.0f,
                ["atkGrowth"] = 100.0f,
                ["defGrowth"] = 63.0f,
                ["speedGrowth"] = 73.0f,
            },
            ["SYLPHI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["TAFFACA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 113.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["TEMPESTRA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 108.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 57.0f,
            },
            ["TEPHRAGON"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 105.0f,
                ["atkGrowth"] = 134.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["TERBITE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 79.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["THERMOFLY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 70.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 58.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["THRANO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 119.0f,
                ["atkGrowth"] = 128.0f,
                ["defGrowth"] = 91.0f,
                ["speedGrowth"] = 51.0f,
            },
            ["TIKALA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 132f,
                ["atkGrowth"] = 154f,
                ["defGrowth"] = 104f,
                ["speedGrowth"] = 55.0f,
            },
            ["TINITO"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["TIRTRAUMA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 115.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 88.0f,
                ["speedGrowth"] = 53.0f,
            },
            ["TITAN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 135f,
                ["atkGrowth"] = 156f,
                ["defGrowth"] = 110f,
                ["speedGrowth"] = 50.0f,
            },
            ["TOKA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 122.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["TOMATHORN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 103.0f,
                ["atkGrowth"] = 116.0f,
                ["defGrowth"] = 80.0f,
                ["speedGrowth"] = 60.0f,
            },
            ["TORNADYR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["TORREX"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 110.0f,
                ["atkGrowth"] = 136.0f,
                ["defGrowth"] = 85.0f,
                ["speedGrowth"] = 56.0f,
            },
            ["TOXIPORE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 54.0f,
                ["atkGrowth"] = 93.0f,
                ["defGrowth"] = 47.0f,
                ["speedGrowth"] = 84.0f,
            },
            ["TRAMPOLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 97.0f,
                ["atkGrowth"] = 120.0f,
                ["defGrowth"] = 76.0f,
                ["speedGrowth"] = 63.0f,
            },
            ["TREEMI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 114.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["TRIMION"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 114.0f,
                ["atkGrowth"] = 123.0f,
                ["defGrowth"] = 87.0f,
                ["speedGrowth"] = 54.0f,
            },
            ["TRONDLE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["TROPI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["TROPITE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 117.0f,
                ["atkGrowth"] = 124.0f,
                ["defGrowth"] = 90.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["TRYFLAP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 60.0f,
                ["atkGrowth"] = 106.0f,
                ["defGrowth"] = 52.0f,
                ["speedGrowth"] = 81.0f,
            },
            ["TURBINHA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 59.0f,
                ["atkGrowth"] = 97.0f,
                ["defGrowth"] = 51.0f,
                ["speedGrowth"] = 82.0f,
            },
            ["TWILLOW"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 75.0f,
                ["atkGrowth"] = 102.0f,
                ["defGrowth"] = 62.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["TYCROC"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 107.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 83.0f,
                ["speedGrowth"] = 58.0f,
            },
            ["TYREGG"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 68.0f,
                ["atkGrowth"] = 105.0f,
                ["defGrowth"] = 57.0f,
                ["speedGrowth"] = 77.0f,
            },
            ["VANAROSTA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 75.0f,
                ["atkGrowth"] = 111.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["VAPRAT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 126.0f,
                ["defGrowth"] = 68.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["VELOKITTI"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 87.0f,
                ["atkGrowth"] = 133.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 68.0f,
            },
            ["VENEMONA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 74.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["VENOWAR"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 74.0f,
                ["atkGrowth"] = 99.0f,
                ["defGrowth"] = 61.0f,
                ["speedGrowth"] = 74.0f,
            },
            ["VENTOFAWN"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 101.0f,
                ["atkGrowth"] = 121.0f,
                ["defGrowth"] = 79.0f,
                ["speedGrowth"] = 61.0f,
            },
            ["VENTRA"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 122.0f,
                ["atkGrowth"] = 137.0f,
                ["defGrowth"] = 93.0f,
                ["speedGrowth"] = 50.0f,
            },
            ["VIVIZARD"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 104.0f,
                ["atkGrowth"] = 135.0f,
                ["defGrowth"] = 81.0f,
                ["speedGrowth"] = 59.0f,
            },
            ["VOLTOSFERE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 95.0f,
                ["atkGrowth"] = 130.0f,
                ["defGrowth"] = 75.0f,
                ["speedGrowth"] = 64.0f,
            },
            ["VORAPEST"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 79.0f,
                ["atkGrowth"] = 114.0f,
                ["defGrowth"] = 64.0f,
                ["speedGrowth"] = 72.0f,
            },
            ["VOZETTE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 73.0f,
                ["atkGrowth"] = 110.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
            ["VULAZY"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 88.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["VULPEP"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 88.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 70.0f,
                ["speedGrowth"] = 67.0f,
            },
            ["VULTIC"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 62.0f,
                ["atkGrowth"] = 109.0f,
                ["defGrowth"] = 53.0f,
                ["speedGrowth"] = 80.0f,
            },
            ["WALNORMOUS"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 116.0f,
                ["atkGrowth"] = 103.0f,
                ["defGrowth"] = 89.0f,
                ["speedGrowth"] = 52.0f,
            },
            ["WEABRIDE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 71.0f,
                ["atkGrowth"] = 104.0f,
                ["defGrowth"] = 59.0f,
                ["speedGrowth"] = 76.0f,
            },
            ["WEAVOLT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 92.0f,
                ["atkGrowth"] = 119.0f,
                ["defGrowth"] = 73.0f,
                ["speedGrowth"] = 65.0f,
            },
            ["WISELIE"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 84.0f,
                ["atkGrowth"] = 127.0f,
                ["defGrowth"] = 67.0f,
                ["speedGrowth"] = 69.0f,
            },
            ["ZIEGLER"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 137f,
                ["atkGrowth"] = 158f,
                ["defGrowth"] = 106f,
                ["speedGrowth"] = 58.0f,
            },
            ["ZOMBYANT"] = new Dictionary<string, float>
            {
                ["hpGrowth"] = 72.0f,
                ["atkGrowth"] = 106.0f,
                ["defGrowth"] = 60.0f,
                ["speedGrowth"] = 75.0f,
            },
        };
    }
}
