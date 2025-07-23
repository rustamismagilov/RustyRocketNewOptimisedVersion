using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScenesTeleporter: MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag ("Player") || other.CompareTag("AlienFriend") || other.CompareTag("Enemy"))
      {
         if (CompareTag("PortalStorageRoom"))
         {
            SceneManager.LoadScene("StorageRoom");
         }

         else if (CompareTag("PortalPlanet1"))

         {
            SceneManager.LoadScene("Planet1");
         }

         else if (CompareTag("PortalPinkRoom"))
         {
            SceneManager.LoadScene("PinkRoom");
         }
      }
   }
}
