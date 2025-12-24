# Level Design Roadmap - First 50 Levels

This document outlines the progression and design metrics for the first 50 levels of Match Factory.

## Design Philosophy

The level progression follows a carefully crafted difficulty curve:
- **Levels 1-10**: Tutorial phase - Simple goals, single item types, generous time
- **Levels 11-20**: Gradual increase - Introducing multiple goal types, reduced time
- **Levels 21-30**: Complexity increase - More items, varied types, challenging time limits
- **Levels 31-40**: Advanced phase - Multiple goals, high item counts, strict time management
- **Levels 41-50**: Expert phase - Maximum complexity, diverse item types, ultimate challenge

## Item Types Available
- Bomb
- Blue_Potion
- Coin
- Cube
- Key

## Level Specifications

| Level | Goal Types | Goal Items | Goal Amounts | Duration (s) | Total Items | Total Item Types | Notes |
|-------|-----------|-----------|--------------|--------------|-------------|------------------|-------|
| 1 | Coin | 1 | 3 | 60 | 9 | 2 | Tutorial: Simple single goal |
| 2 | Bomb | 1 | 3 | 60 | 9 | 2 | Introduction to Bomb |
| 3 | Cube | 1 | 6 | 60 | 12 | 2 | Slightly increased goal |
| 4 | Blue_Potion | 1 | 6 | 55 | 12 | 2 | New item type |
| 5 | Key | 1 | 6 | 55 | 15 | 3 | Introduction to Key |
| 6 | Coin | 1 | 9 | 55 | 15 | 3 | Higher goal amount |
| 7 | Bomb, Coin | 2 | 3, 3 | 70 | 18 | 3 | First multi-goal level |
| 8 | Cube, Blue_Potion | 2 | 3, 6 | 70 | 18 | 3 | Mixed goal amounts |
| 9 | Key, Bomb | 2 | 6, 6 | 65 | 21 | 4 | More items to manage |
| 10 | Coin, Cube | 2 | 6, 9 | 65 | 24 | 4 | End of tutorial phase |
| 11 | Blue_Potion, Key | 2 | 9, 6 | 70 | 27 | 4 | Increasing complexity |
| 12 | Bomb, Coin, Cube | 3 | 3, 3, 3 | 80 | 30 | 4 | Three goals |
| 13 | Key, Blue_Potion | 2 | 9, 9 | 65 | 30 | 4 | Higher goal amounts |
| 14 | Coin, Bomb, Key | 3 | 6, 3, 6 | 75 | 33 | 5 | All item types present |
| 15 | Cube, Blue_Potion, Bomb | 3 | 6, 6, 3 | 75 | 33 | 5 | Varied distribution |
| 16 | Coin, Key | 2 | 12, 9 | 70 | 36 | 5 | High goal amounts |
| 17 | Bomb, Cube, Blue_Potion | 3 | 6, 6, 6 | 80 | 39 | 5 | Equal distribution |
| 18 | Key, Coin, Bomb | 3 | 9, 6, 6 | 75 | 39 | 5 | Time pressure increase |
| 19 | Blue_Potion, Cube | 2 | 12, 12 | 70 | 42 | 5 | Very high goals |
| 20 | All 5 types | 5 | 3, 3, 3, 3, 3 | 90 | 45 | 5 | Milestone: All types as goals |
| 21 | Coin, Bomb, Key | 3 | 9, 9, 6 | 75 | 45 | 5 | Phase 3 begins |
| 22 | Cube, Blue_Potion, Coin | 3 | 9, 9, 9 | 80 | 48 | 5 | High equal goals |
| 23 | Bomb, Key, Blue_Potion | 3 | 6, 12, 6 | 75 | 48 | 5 | Focus on one item |
| 24 | Coin, Cube, Bomb, Key | 4 | 6, 6, 3, 6 | 85 | 51 | 5 | Four goals |
| 25 | All 5 types | 5 | 6, 3, 6, 3, 6 | 90 | 54 | 5 | Complex multi-goal |
| 26 | Blue_Potion, Coin, Cube | 3 | 12, 9, 9 | 80 | 54 | 5 | Very high amounts |
| 27 | Key, Bomb, Blue_Potion | 3 | 12, 6, 12 | 75 | 57 | 5 | Challenging time |
| 28 | Coin, Cube, Key, Bomb | 4 | 9, 6, 6, 6 | 85 | 57 | 5 | Four-way challenge |
| 29 | All 5 types | 5 | 6, 6, 6, 6, 6 | 90 | 60 | 5 | Equal five-way |
| 30 | Blue_Potion, Coin | 2 | 15, 15 | 75 | 60 | 5 | High dual goals |
| 31 | Bomb, Cube, Key, Blue_Potion | 4 | 9, 9, 6, 9 | 85 | 63 | 5 | Advanced phase begins |
| 32 | All 5 types | 5 | 6, 6, 9, 6, 6 | 90 | 66 | 5 | All with variation |
| 33 | Coin, Key, Bomb | 3 | 15, 9, 9 | 80 | 66 | 5 | Very high lead goal |
| 34 | Cube, Blue_Potion, Coin, Key | 4 | 9, 9, 9, 6 | 85 | 69 | 5 | Four high goals |
| 35 | All 5 types | 5 | 9, 6, 6, 6, 9 | 95 | 72 | 5 | Milestone level |
| 36 | Bomb, Coin, Cube | 3 | 12, 12, 12 | 80 | 72 | 5 | Triple high goals |
| 37 | Key, Blue_Potion, Bomb, Coin | 4 | 12, 9, 6, 9 | 90 | 75 | 5 | Mixed high amounts |
| 38 | All 5 types | 5 | 9, 9, 6, 9, 6 | 95 | 78 | 5 | Nearly equal five |
| 39 | Cube, Key, Blue_Potion | 3 | 15, 12, 12 | 85 | 78 | 5 | Very high challenge |
| 40 | All 5 types | 5 | 9, 9, 9, 9, 6 | 95 | 81 | 5 | Phase checkpoint |
| 41 | Coin, Bomb, Cube, Key, Blue_Potion | 5 | 12, 6, 9, 9, 9 | 95 | 84 | 5 | Expert phase begins |
| 42 | Bomb, Cube, Blue_Potion | 3 | 15, 15, 12 | 85 | 84 | 5 | Triple expert goals |
| 43 | All 5 types | 5 | 9, 9, 9, 9, 9 | 100 | 87 | 5 | Perfect equality |
| 44 | Coin, Key, Bomb, Cube | 4 | 15, 12, 9, 9 | 90 | 87 | 5 | Very high quad |
| 45 | All 5 types | 5 | 12, 9, 9, 9, 9 | 100 | 90 | 5 | High five-way |
| 46 | Blue_Potion, Coin, Key | 3 | 18, 15, 12 | 90 | 90 | 5 | Extreme goals |
| 47 | All 5 types | 5 | 12, 9, 12, 9, 9 | 100 | 93 | 5 | Near maximum |
| 48 | Bomb, Cube, Coin, Blue_Potion | 4 | 15, 15, 12, 12 | 95 | 93 | 5 | Quad extreme |
| 49 | All 5 types | 5 | 12, 12, 9, 12, 9 | 100 | 96 | 5 | Final challenge prep |
| 50 | All 5 types | 5 | 15, 12, 12, 12, 12 | 105 | 99 | 5 | Ultimate challenge |

## Progression Metrics

### Goal Progression
- **Early Game (1-10)**: Start with 3-9 items per goal, 1-2 goal types
- **Mid Game (11-30)**: Scale to 9-15 items per goal, 2-5 goal types
- **Late Game (31-50)**: Peak at 12-18 items per goal (maximum 18), 3-5 goal types

### Duration Progression
- **Levels 1-10**: 55-70 seconds (learning phase)
- **Levels 11-20**: 65-90 seconds (skill building)
- **Levels 21-30**: 70-90 seconds (complexity management)
- **Levels 31-40**: 75-95 seconds (advanced challenge)
- **Levels 41-50**: 85-105 seconds (expert endurance)

### Item Amount Progression
- **Total items scale from 9 (Level 1) to 99 (Level 50)**
- Average progression: approximately +1.84 items per level
- Actual progression varies (0-3 items increase per level) to fine-tune difficulty
- Ensures steady difficulty increase
- All amounts are multiples of 3 (matching game constraint)

### Item Type Diversity
- **Levels 1-5**: 2-3 types (introduction)
- **Levels 6-13**: 3-4 types (building variety)
- **Levels 14-50**: All 5 types present in total items (full complexity)
- **Level 20+**: Milestone - first occurrence of all 5 types as goal items simultaneously

## Design Constraints

1. **Amount Constraint**: All item amounts must be multiples of 3 (matching mechanic)
2. **Goal Items**: Goals are a subset of total items available in the level
3. **Time Balance**: Duration increases with item count to maintain playability
4. **Type Variety**: All 5 item types present in total items by level 14, all as goals by level 20

## Difficulty Factors

Each level's difficulty is determined by:
1. **Number of goal types** (1-5): More goals = more complex
2. **Goal amounts**: Higher numbers require more matches
3. **Time pressure**: Shorter durations increase difficulty
4. **Total items**: More items = more sorting required
5. **Distribution**: Unequal goals can be harder than equal ones

## Level Milestones

- **Level 10**: Completion of tutorial phase
- **Level 20**: First time all 5 types are goals
- **Level 30**: Mid-game checkpoint with high dual goals
- **Level 40**: Advanced phase completion
- **Level 50**: Ultimate challenge with maximum complexity

## Implementation Notes

- Each level should be tested for completion within the time limit
- Consider adding powerups for levels 30+
- Monitor player analytics to adjust difficulty if needed
- Levels can loop/repeat after level 50 with randomized parameters
