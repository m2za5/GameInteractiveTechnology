# Eye-Tracking Based Adaptive Brightness Control Interface

Unity에서 Tobii Eye Tracker를 연동하여, 사용자의 시선 데이터를 기반으로 실시간 밝기를 자동 조절하는 시스템입니다.

## 프로젝트 개요

기존의 수동 밝기 조절 방식은 조작이 번거롭고, 사용자마다 시각 인지 능력에 차이가 있어 일관된 경험을 제공하기 어렵습니다. 본 프로젝트는 사용자의 시선을 추적해 오브젝트를 인식할 수 있는 밝기의 임계점을 파악하고, 그 정보를 바탕으로 자동으로 밝기를 조정하는 시스템을 설계했습니다.

## 기술 스택

- **Engine:** Unity Editor 2022.3.21f1
- **Eye Tracker:** Tobii Eye Tracker 5
- **Language:** C#

## 실험 구성

세 가지 조건에서 자동 조절 기능과 기존 방식 간의 정확도 및 사용자 만족도를 비교 평가했습니다.

| 실험 | 방식 | 설명 |
|------|------|------|
| 실험 1 | General Light Control | 슬라이더를 이용한 기존 수동 밝기 조절 |
| 실험 2 | Correlation Method | 아이트래킹 기반 동적 오브젝트 추적. 시선 응시에 따라 밝기가 점진적으로 변화 |
| 실험 3 | Flicker Object Tracking | 아이트래킹 기반 정적 오브젝트 추적. 깜빡이는 오브젝트를 응시하여 밝기 임계점 측정 |

## 실험 결과

| 실험 | 정확도 |
|------|--------|
| 실험 1 (수동 조절) | 72.7% |
| 실험 2 (동적 트래킹) | 79.6% |
| 실험 3 (정적 트래킹) | 92.6% |

시선 기반 자동 밝기 조절 시스템이 기존 수동 방식 대비 인식 정확도와 사용자 만족도에서 개선된 결과를 보였으며, 특히 정적 오브젝트 트래킹 방식이 가장 높은 정확도를 기록했습니다.

## 팀 구성

4인 팀 프로젝트 (경희대학교)

## 참고 문헌

- Sibert & Jacob, "Evaluation of Eye Gaze Interaction," SIGCHI 2000
- Tedla et al., "LookToFocus: Image Focus via Eye Tracking," ETRA 2024
- Su et al., "Readability enhancement of displayed images under ambient light," IEEE TCSVT 2018
- Liao et al., "Deep Retinex Decomposition for Low-Light Enhancement," arXiv 2019
