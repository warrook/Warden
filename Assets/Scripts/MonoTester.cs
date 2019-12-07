using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Warden;
using Generic;

public class MonoTester : MonoBehaviour
{
	private ContentPack pack;
	private BattleController controller;
    // Start is called before the first frame update
    void Start()
    {
		//pack = new ContentPack(new DirectoryInfo(Path.Combine(FileLocation.ContentDir.FullName, "Core" + Path.DirectorySeparatorChar)), "Core");
		ContentLoader.Log();
		ContentLoader.LoadPacks();

		controller = new BattleController();
		//pack.LoadData();

		//Beast wisp = new Beast(Database<BeastData>.GetByName("WillOWisp"), 10);
		//Debug.Log("Wisp -- " + wisp.ToString() + "\n" + string.Join(",", wisp.KnownMoves));
		//Beast pokey = new Beast(Database<BeastData>.GetByName("Needler"), 10);
		//pokey.LevelUp(15);
		//Debug.Log(pokey.Name + " -- " + pokey.ToString() + "\n" + string.Join(",", pokey.KnownMoves));

		//pokey.InstantiateKnownMove(Database<MoveData>.GetByName("Move_Slash"));
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
