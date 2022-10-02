using UnityEngine;
using System.Collections;

public interface IUiController
{
	void Activate(UIController controller);
	void End();
}
