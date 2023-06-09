//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/TankEntitiesMultiplayer/PlayerControls/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace TankEntitiesMultiplayer.PlayerControls
{
    public partial class @PlayerControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""CameraControls"",
            ""id"": ""bbdda887-fe1b-4745-9802-b7eb4284cf18"",
            ""actions"": [
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""9b006ad0-7114-4c33-9d17-864811126bd0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DragDelta"",
                    ""type"": ""Value"",
                    ""id"": ""8a175c16-d0a3-4151-95c6-9b1307be8f1b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""788615dd-70b5-486d-b6d7-918110c4bdba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""4dcc0c17-992a-451c-abad-f1adf80f524e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cdcc254a-453f-48a6-9c70-33c6b4d60e76"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8d9467c-a391-45de-b4d5-3c0026a88b2b"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DragDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b388f90f-c500-463f-98e7-ddeec9403bb8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc1671dc-9411-4b89-8b44-b08af01e49d8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // CameraControls
            m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
            m_CameraControls_Drag = m_CameraControls.FindAction("Drag", throwIfNotFound: true);
            m_CameraControls_DragDelta = m_CameraControls.FindAction("DragDelta", throwIfNotFound: true);
            m_CameraControls_Select = m_CameraControls.FindAction("Select", throwIfNotFound: true);
            m_CameraControls_Action = m_CameraControls.FindAction("Action", throwIfNotFound: true);
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

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // CameraControls
        private readonly InputActionMap m_CameraControls;
        private List<ICameraControlsActions> m_CameraControlsActionsCallbackInterfaces = new List<ICameraControlsActions>();
        private readonly InputAction m_CameraControls_Drag;
        private readonly InputAction m_CameraControls_DragDelta;
        private readonly InputAction m_CameraControls_Select;
        private readonly InputAction m_CameraControls_Action;
        public struct CameraControlsActions
        {
            private @PlayerControls m_Wrapper;
            public CameraControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Drag => m_Wrapper.m_CameraControls_Drag;
            public InputAction @DragDelta => m_Wrapper.m_CameraControls_DragDelta;
            public InputAction @Select => m_Wrapper.m_CameraControls_Select;
            public InputAction @Action => m_Wrapper.m_CameraControls_Action;
            public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
            public void AddCallbacks(ICameraControlsActions instance)
            {
                if (instance == null || m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Add(instance);
                @Drag.started += instance.OnDrag;
                @Drag.performed += instance.OnDrag;
                @Drag.canceled += instance.OnDrag;
                @DragDelta.started += instance.OnDragDelta;
                @DragDelta.performed += instance.OnDragDelta;
                @DragDelta.canceled += instance.OnDragDelta;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
            }

            private void UnregisterCallbacks(ICameraControlsActions instance)
            {
                @Drag.started -= instance.OnDrag;
                @Drag.performed -= instance.OnDrag;
                @Drag.canceled -= instance.OnDrag;
                @DragDelta.started -= instance.OnDragDelta;
                @DragDelta.performed -= instance.OnDragDelta;
                @DragDelta.canceled -= instance.OnDragDelta;
                @Select.started -= instance.OnSelect;
                @Select.performed -= instance.OnSelect;
                @Select.canceled -= instance.OnSelect;
                @Action.started -= instance.OnAction;
                @Action.performed -= instance.OnAction;
                @Action.canceled -= instance.OnAction;
            }

            public void RemoveCallbacks(ICameraControlsActions instance)
            {
                if (m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICameraControlsActions instance)
            {
                foreach (var item in m_Wrapper.m_CameraControlsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CameraControlsActions @CameraControls => new CameraControlsActions(this);
        public interface ICameraControlsActions
        {
            void OnDrag(InputAction.CallbackContext context);
            void OnDragDelta(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
            void OnAction(InputAction.CallbackContext context);
        }
    }
}
