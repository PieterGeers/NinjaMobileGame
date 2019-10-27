//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Character_Controller : MonoBehaviour
//{

//    /*Function Update does the Input and the automatic movement of the character*/
//    private void Update()
//    {
//        HandleInput();

//        if (_start)
//        {
//            float x = GetDistanceToLastPole();
//            if ((x >= _poleManager.GetMaxDistance() && !_grapple) || (x >= _poleManager.GetMaxDistance() * 2f && _grapple))
//            {
//                if (!_start)
//                {
//                    FindObjectOfType<GameState>().GetComponent<GameState>().SetStart(true);
//                    _start = true;
//                }
//                _grapple = false;
//                ++_score;
//                if (_poleManager.GetNexPole().tag == "DefaultPole")
//                    CalculateQuadraticParam();
//                else
//                {
//                    _grapple = true;
//                    CalculateQuadraticGrapplingParam();
//                }
//            }
//            else if (x <= _poleManager.GetMaxDistance() && !_grapple)
//            {
//                MoveQuadratic(x, _quadraticParamFase2);
//            }
//            else if (x <= _poleManager.GetMaxDistance() * 2f && _grapple)
//            {
//                if (x <= _grapplingOffset)
//                {
//                    MoveQuadratic(x, _quadraticParamFase1);
//                }
//                else if (x >= (_poleManager.GetMaxDistance() * 2f) - _grapplingOffset)
//                {
//                    if (_prevGrapplingPole != null)
//                        Destroy(_prevGrapplingPole);
//                    MoveQuadratic(x, _quadraticParamFase3);
//                }
//                else
//                {
//                    if (!_pressed)
//                        _start = false;
//                    MoveQuadratic(x, _quadraticParamFase2);
//                    DrawLineWhileGrapple();
//                }
//            }
//        }
//        else if (_prevGrapplingPole != null)
//        {
//            Destroy(_prevGrapplingPole);
//        }
//    }
//}
