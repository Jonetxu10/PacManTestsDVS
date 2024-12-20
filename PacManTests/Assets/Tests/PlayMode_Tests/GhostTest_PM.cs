using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;
using System.Collections;
using System.Collections.Generic;

/* NOMBRE CLASE: GhostTest_PM
 * AUTOR: Jone Sainz Egea
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 test y todo lo necesario para que funcione
 * DESCRIPCIÓN: Comprueba que al resetear el estado de Ghost se activa el comportamiento inicial
 */
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
    public IEnumerator ResetStateEnablesInitialBehavior()
    {
        ghostGameObject.SetActive(true);

        ghost.ResetState();
        yield return null;

        Assert.IsTrue(ghost.scatter.enabled, "El estado Scatter no se ha habilitado.");
        Assert.IsFalse(ghost.chase.enabled, "El estado Chase no se ha deshabilitado.");
        Assert.IsFalse(ghost.frightened.enabled, "El estado Frightened no se ha deshabilitado.");
        Assert.IsFalse(ghost.home.enabled, "El estado Home no se ha deshabilitado.");
    }
}