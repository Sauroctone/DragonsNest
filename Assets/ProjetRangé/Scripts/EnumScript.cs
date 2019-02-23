using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcherGroupState { MOVING, SHOOTING, FLEEING_INDIVIDUALLY }
public enum BallistaState { MOVING, SHOOTING }
public enum WaveState { TUTO, DURING_WAVE, WAITING_FOR_LAST_ENEMIES, RESTING }
public enum EnemyTargetType { EGG, ANCIENT, PLAYER }