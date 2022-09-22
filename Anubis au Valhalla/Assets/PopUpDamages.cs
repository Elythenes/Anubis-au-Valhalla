using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpDamages : MonoBehaviour
{
   private void Update()
   {
      Destroy(transform.parent.gameObject,1f);
   }
}
