// GENERATED AUTOMATICALLY FROM 'Assets/Input/UI_InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class UI_InputController : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public UI_InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UI_InputController"",
    ""maps"": [
        {
            ""name"": ""UIMainmenuCtrl"",
            ""id"": ""fb1c64f5-7d88-4906-aebc-a78ed66b1f3f"",
            ""actions"": [
                {
                    ""name"": ""ClickTitleCover"",
                    ""type"": ""Button"",
                    ""id"": ""15ecc827-9ee3-4ca3-a3e1-59d1fcd42f2e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""test"",
                    ""type"": ""Button"",
                    ""id"": ""84aebf9f-65ef-48ee-a78f-ceba7d3984df"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bfc56ebc-0b9e-483a-a793-cf4a7b1da34a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""ClickTitleCover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22ec2315-45cc-4812-8991-56e3256a6e6b"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""ClickTitleCover"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7151a310-d39a-4549-9af3-623ae9e4e849"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""keyboard"",
            ""bindingGroup"": ""keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // UIMainmenuCtrl
        m_UIMainmenuCtrl = asset.FindActionMap("UIMainmenuCtrl", throwIfNotFound: true);
        m_UIMainmenuCtrl_ClickTitleCover = m_UIMainmenuCtrl.FindAction("ClickTitleCover", throwIfNotFound: true);
        m_UIMainmenuCtrl_test = m_UIMainmenuCtrl.FindAction("test", throwIfNotFound: true);
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

    // UIMainmenuCtrl
    private readonly InputActionMap m_UIMainmenuCtrl;
    private IUIMainmenuCtrlActions m_UIMainmenuCtrlActionsCallbackInterface;
    private readonly InputAction m_UIMainmenuCtrl_ClickTitleCover;
    private readonly InputAction m_UIMainmenuCtrl_test;
    public struct UIMainmenuCtrlActions
    {
        private UI_InputController m_Wrapper;
        public UIMainmenuCtrlActions(UI_InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @ClickTitleCover => m_Wrapper.m_UIMainmenuCtrl_ClickTitleCover;
        public InputAction @test => m_Wrapper.m_UIMainmenuCtrl_test;
        public InputActionMap Get() { return m_Wrapper.m_UIMainmenuCtrl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIMainmenuCtrlActions set) { return set.Get(); }
        public void SetCallbacks(IUIMainmenuCtrlActions instance)
        {
            if (m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface != null)
            {
                ClickTitleCover.started -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnClickTitleCover;
                ClickTitleCover.performed -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnClickTitleCover;
                ClickTitleCover.canceled -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnClickTitleCover;
                test.started -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnTest;
                test.performed -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnTest;
                test.canceled -= m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface.OnTest;
            }
            m_Wrapper.m_UIMainmenuCtrlActionsCallbackInterface = instance;
            if (instance != null)
            {
                ClickTitleCover.started += instance.OnClickTitleCover;
                ClickTitleCover.performed += instance.OnClickTitleCover;
                ClickTitleCover.canceled += instance.OnClickTitleCover;
                test.started += instance.OnTest;
                test.performed += instance.OnTest;
                test.canceled += instance.OnTest;
            }
        }
    }
    public UIMainmenuCtrlActions @UIMainmenuCtrl => new UIMainmenuCtrlActions(this);
    private int m_keyboardSchemeIndex = -1;
    public InputControlScheme keyboardScheme
    {
        get
        {
            if (m_keyboardSchemeIndex == -1) m_keyboardSchemeIndex = asset.FindControlSchemeIndex("keyboard");
            return asset.controlSchemes[m_keyboardSchemeIndex];
        }
    }
    public interface IUIMainmenuCtrlActions
    {
        void OnClickTitleCover(InputAction.CallbackContext context);
        void OnTest(InputAction.CallbackContext context);
    }
}
