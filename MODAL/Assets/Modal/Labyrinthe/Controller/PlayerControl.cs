// GENERATED AUTOMATICALLY FROM 'Assets/Modal/Labyrinthe/Controller/PlayerControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ModalFunctions.Controller
{
    public class @PlayerControl : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControl()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""MovePlayer"",
            ""id"": ""9c59426e-5fdc-43b7-a4a7-6dbe300fda34"",
            ""actions"": [
                {
                    ""name"": ""MovePlayer"",
                    ""type"": ""Button"",
                    ""id"": ""5a20c3c5-39a7-429a-8956-de4e7c96a97e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Button"",
                    ""type"": ""Button"",
                    ""id"": ""a9a523f8-7b0a-4cd0-97ec-9d909c171ff9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""90ca58ca-a98e-48f7-b1a6-a0bbff561a26"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovePlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""434afc3d-451a-4e9d-b2b4-382b3fa63a94"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MoveFreeLookCamera"",
            ""id"": ""374ad87b-2537-4862-bbd7-42f34d62ae78"",
            ""actions"": [
                {
                    ""name"": ""MoveCamera"",
                    ""type"": ""Button"",
                    ""id"": ""df97ad58-3be0-4e05-93c8-68ddf0c35b0c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4c564a6-7f25-42ea-999f-4d6a91a8c2c1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // MovePlayer
            m_MovePlayer = asset.FindActionMap("MovePlayer", throwIfNotFound: true);
            m_MovePlayer_MovePlayer = m_MovePlayer.FindAction("MovePlayer", throwIfNotFound: true);
            m_MovePlayer_Button = m_MovePlayer.FindAction("Button", throwIfNotFound: true);
            // MoveFreeLookCamera
            m_MoveFreeLookCamera = asset.FindActionMap("MoveFreeLookCamera", throwIfNotFound: true);
            m_MoveFreeLookCamera_MoveCamera = m_MoveFreeLookCamera.FindAction("MoveCamera", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // MovePlayer
        private readonly InputActionMap m_MovePlayer;
        private IMovePlayerActions m_MovePlayerActionsCallbackInterface;
        private readonly InputAction m_MovePlayer_MovePlayer;
        private readonly InputAction m_MovePlayer_Button;
        public struct MovePlayerActions
        {
            private @PlayerControl m_Wrapper;
            public MovePlayerActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @MovePlayer => m_Wrapper.m_MovePlayer_MovePlayer;
            public InputAction @Button => m_Wrapper.m_MovePlayer_Button;
            public InputActionMap Get() { return m_Wrapper.m_MovePlayer; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MovePlayerActions set) { return set.Get(); }
            public void SetCallbacks(IMovePlayerActions instance)
            {
                if (m_Wrapper.m_MovePlayerActionsCallbackInterface != null)
                {
                    @MovePlayer.started -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnMovePlayer;
                    @MovePlayer.performed -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnMovePlayer;
                    @MovePlayer.canceled -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnMovePlayer;
                    @Button.started -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnButton;
                    @Button.performed -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnButton;
                    @Button.canceled -= m_Wrapper.m_MovePlayerActionsCallbackInterface.OnButton;
                }
                m_Wrapper.m_MovePlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MovePlayer.started += instance.OnMovePlayer;
                    @MovePlayer.performed += instance.OnMovePlayer;
                    @MovePlayer.canceled += instance.OnMovePlayer;
                    @Button.started += instance.OnButton;
                    @Button.performed += instance.OnButton;
                    @Button.canceled += instance.OnButton;
                }
            }
        }
        public MovePlayerActions @MovePlayer => new MovePlayerActions(this);

        // MoveFreeLookCamera
        private readonly InputActionMap m_MoveFreeLookCamera;
        private IMoveFreeLookCameraActions m_MoveFreeLookCameraActionsCallbackInterface;
        private readonly InputAction m_MoveFreeLookCamera_MoveCamera;
        public struct MoveFreeLookCameraActions
        {
            private @PlayerControl m_Wrapper;
            public MoveFreeLookCameraActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveCamera => m_Wrapper.m_MoveFreeLookCamera_MoveCamera;
            public InputActionMap Get() { return m_Wrapper.m_MoveFreeLookCamera; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MoveFreeLookCameraActions set) { return set.Get(); }
            public void SetCallbacks(IMoveFreeLookCameraActions instance)
            {
                if (m_Wrapper.m_MoveFreeLookCameraActionsCallbackInterface != null)
                {
                    @MoveCamera.started -= m_Wrapper.m_MoveFreeLookCameraActionsCallbackInterface.OnMoveCamera;
                    @MoveCamera.performed -= m_Wrapper.m_MoveFreeLookCameraActionsCallbackInterface.OnMoveCamera;
                    @MoveCamera.canceled -= m_Wrapper.m_MoveFreeLookCameraActionsCallbackInterface.OnMoveCamera;
                }
                m_Wrapper.m_MoveFreeLookCameraActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveCamera.started += instance.OnMoveCamera;
                    @MoveCamera.performed += instance.OnMoveCamera;
                    @MoveCamera.canceled += instance.OnMoveCamera;
                }
            }
        }
        public MoveFreeLookCameraActions @MoveFreeLookCamera => new MoveFreeLookCameraActions(this);
        public interface IMovePlayerActions
        {
            void OnMovePlayer(InputAction.CallbackContext context);
            void OnButton(InputAction.CallbackContext context);
        }
        public interface IMoveFreeLookCameraActions
        {
            void OnMoveCamera(InputAction.CallbackContext context);
        }
    }
}
