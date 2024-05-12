/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_BGM = 3126765036U;
        static const AkUniqueID PLAY_CHARASELECTED = 1209185904U;
        static const AkUniqueID PLAY_GAMEOVER = 3174629258U;
        static const AkUniqueID PLAY_HIT = 2960666077U;
        static const AkUniqueID PLAY_SCORE = 3240049516U;
        static const AkUniqueID PLAY_SPEEDBOOST = 3080395584U;
        static const AkUniqueID PLAY_SWING = 989180208U;
        static const AkUniqueID SET_STATE_BGM_COMBAT = 999787593U;
        static const AkUniqueID SET_STATE_BGM_COMBAT_ENTER = 2302536382U;
        static const AkUniqueID SET_STATE_BGM_COMBAT_MAIN = 1593948787U;
        static const AkUniqueID SET_STATE_BGM_COMBAT_NONE = 3874713034U;
        static const AkUniqueID SET_STATE_BGM_MENU = 3051978052U;
        static const AkUniqueID SET_STATE_BGM_NONE = 1198540079U;
        static const AkUniqueID SET_STATE_BGM_SELECTION = 3642539605U;
        static const AkUniqueID STOP_BGM = 1073466678U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace BGM
        {
            static const AkUniqueID GROUP = 412724365U;

            namespace STATE
            {
                static const AkUniqueID COMBAT = 2764240573U;
                static const AkUniqueID MENU = 2607556080U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SELECTION = 1973847113U;
            } // namespace STATE
        } // namespace BGM

        namespace BGM_COMBAT
        {
            static const AkUniqueID GROUP = 736197432U;

            namespace STATE
            {
                static const AkUniqueID ENTER = 2368844905U;
                static const AkUniqueID MAIN = 3161908922U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace BGM_COMBAT

    } // namespace STATES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINSOUNDBANK = 534561221U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
