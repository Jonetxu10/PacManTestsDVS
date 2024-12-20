using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;
using System.Collections;
using System.Collections.Generic;

public class GhostTest_PM
{
    private GameObject pacmanGameObject;
    private GameObject ghostGameObject;
    private Ghost ghost;

    [SetUp]
    public void Setup()
    {
        pacmanGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Pacman"), new Vector3(0, 0, 0), Quaternion.identity);
        ghostGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ghost_Base"), new Vector3(20, 0, 0), Quaternion.identity);
        ghost = ghostGameObject.GetComponent<Ghost>();

    }

    [UnityTest]
    public IEnumerator ResetState_ShouldEnableInitialBehavior()
    {
        // Activa el GameObject para simular el estado en el juego
        ghostGameObject.SetActive(true);

        // Llama al método ResetState
        ghost.ResetState();

        // Espera un frame para permitir la ejecución en Play Mode
        yield return null;

        // Comprueba que los estados iniciales son correctos
        Assert.IsTrue(ghost.scatter.enabled, "El estado Scatter debería estar habilitado.");
        Assert.IsFalse(ghost.chase.enabled, "El estado Chase debería estar deshabilitado.");
        Assert.IsFalse(ghost.frightened.enabled, "El estado Frightened debería estar deshabilitado.");
        Assert.IsFalse(ghost.home.enabled, "El estado Home debería estar deshabilitado si no es el comportamiento inicial.");
    }
}