using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class pahtfind_test
{
    [UnityTest]
    public IEnumerator CameraMovementUpTest()
    {
        // The test checks if the camera moves up if keyboard input is recieved

        // Creates the camera object
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        Vector3 originalPosition = camera.transform.position;
        camera.transform.tag = "test";

        // Creates the player controller
        GameObject player = new GameObject();
        PlayerControl control = player.AddComponent<PlayerControl>();
        control.pos.z++;
        player.transform.tag = "test";

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the camera moved up
        Assert.AreEqual(new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + 1), camera.transform.position);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator CameraMovementDownTest()
    {
        // The test checks if the camera moves down if keyboard input is recieved

        // Creates the camera object
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        Vector3 originalPosition = camera.transform.position;
        camera.transform.tag = "test";

        // Creates the player controller
        GameObject player = new GameObject();
        PlayerControl control = player.AddComponent<PlayerControl>();
        control.pos.z--;
        player.transform.tag = "test";

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the camera moved down
        Assert.AreEqual(new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - 1), camera.transform.position);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator CameraMovementLeftTest()
    {
        // The test checks if the camera moves left if keyboard input is recieved

        // Creates the camera object
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        Vector3 originalPosition = camera.transform.position;
        camera.transform.tag = "test";

        // Creates the player controller
        GameObject player = new GameObject();
        PlayerControl control = player.AddComponent<PlayerControl>();
        control.pos.x--;
        player.transform.tag = "test";

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the camera moved left
        Assert.AreEqual(new Vector3(originalPosition.x - 1, originalPosition.y, originalPosition.z), camera.transform.position);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator CameraMovementRightTest()
    {
        // The test checks if the camera moves right if keyboard input is recieved

        // Creates the camera object
        GameObject camera = new GameObject();
        camera.AddComponent<Camera>();
        Vector3 originalPosition = camera.transform.position;
        camera.transform.tag = "test";

        // Creates the player controller
        GameObject player = new GameObject();
        PlayerControl control = player.AddComponent<PlayerControl>();
        control.pos.x++;
        player.transform.tag = "test";

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the camera moved right
        Assert.AreEqual(new Vector3(originalPosition.x + 1, originalPosition.y, originalPosition.z), camera.transform.position);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator ScrollTest()
    {
        // The test checks if the camera orthographic size changes when the mouse wheel change is recieved

        // Creates the camera object
        GameObject camera = new GameObject();
        Camera theCamera = camera.AddComponent<Camera>();
        float originalSize = theCamera.orthographicSize;
        camera.transform.tag = "test";

        // Creates the player controller
        GameObject player = new GameObject();
        PlayerControl control = player.AddComponent<PlayerControl>();
        player.transform.tag = "test";
        control.scroll -= 10f;

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the orthographic size of the camera has been changed from the original
        Assert.AreNotEqual(originalSize, theCamera.orthographicSize);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator FogRemoveTest()
    {
        // This test checks if in collision the fog is removed
        
        // Creates the unit prototype
        GameObject player = new GameObject();
        player.AddComponent<Rigidbody>();
        SphereCollider collider = player.AddComponent<SphereCollider>();
        Vector3 thePosition = player.transform.position;
        collider.isTrigger = true;
        player.transform.tag = "test";

        // Creates the fog prototype gameobject
        GameObject fog = new GameObject();
        GameObject sprite = new GameObject();
        sprite.transform.parent = fog.transform;
        SpriteRenderer render = sprite.AddComponent<SpriteRenderer>();
        FogData data = fog.AddComponent<FogData>();
        fog.AddComponent<BoxCollider>();
        fog.transform.tag = "test";
        data.halfFog = null;
        data.fullFog = null;
        data.sprite = render;
        fog.transform.position = thePosition;

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the fog sprite rendere is disabled on collision
        Assert.IsFalse(render.enabled);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator FogMapCreateTest()
    {
        // This test checks if the fogmap is created in the correct size
        
        // Creates the fog generator prototype object
        GameObject theObject = new GameObject();
        theObject.transform.tag = "test";
        FogOfWar fogOfWar = theObject.AddComponent<FogOfWar>();
        GameObject fog = new GameObject();
        fog.transform.tag = "test";
        fogOfWar.Fog = fog;
        int size = 5;
        fogOfWar.Createfog(size);

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Checks if the fogmap is created in the correct size
        Assert.AreEqual(size * size, theObject.transform.childCount);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return true;
    }

    [UnityTest]
    public IEnumerator FogMapRemoveTest()
    {
        // This test checks if the fogmap is removed successfully

        // Creates the fog generator prototype object
        GameObject theObject = new GameObject();
        theObject.transform.tag = "test";
        FogOfWar fogOfWar = theObject.AddComponent<FogOfWar>();
        GameObject fog = new GameObject();
        fog.transform.tag = "test";
        fogOfWar.Fog = fog;
        int size = 5;
        fogOfWar.Createfog(size);

        // Wait for process to be made in-game
        yield return new WaitForSeconds(1);

        // Removes the fogmap
        fogOfWar.deleteFogMap();

        // Checks if the fogmap is actually removed
        Assert.AreEqual(0, theObject.transform.childCount);

        // Cleans up the scene
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("test"))
        {
            Object.Destroy(temp);
        }

        yield return null;
    }
}
