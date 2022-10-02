using UnityEngine;
using System.Collections;

public interface IPhase
{
	void StartPhase(Controller controller);
	bool EndPhase();
}
