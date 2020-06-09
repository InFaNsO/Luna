// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputController : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""PlayerControl"",
            ""id"": ""515255e0-b1ef-4d98-ac22-7591cbc7eff1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""47322d7b-286e-4441-a10c-0802750a420c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""ed3bc27c-7c3e-48e5-bb43-009e8f98d796"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""d18f930a-d084-48a3-8b15-ddf27c5958f4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DropWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""96d5f085-6e3c-456f-b9f8-ee5c28e5105d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""e7afbe44-2fbf-4bf9-99dd-d812051421eb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""5db48c35-4a10-4608-9c67-9b8eaad0651b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""d9424452-2a4b-480a-a0e3-34a53cf624a7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectPrevItem"",
                    ""type"": ""Button"",
                    ""id"": ""d3e6c636-3e09-4dce-a0ca-0d613efbc649"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectNextItem"",
                    ""type"": ""Button"",
                    ""id"": ""f96bdf43-a135-4682-a47b-081bbf447e50"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PickUpWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""835b573c-bda4-475b-8c8c-442e34ca7163"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Parry"",
                    ""type"": ""Button"",
                    ""id"": ""fe300ee3-dadd-4599-8c3f-b8cef635f18d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8ace9d09-d120-4a68-b591-b285f52edc6c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44188d62-2ab7-457b-9bff-b33b5ede1cf1"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb9946c6-6141-4eaf-a527-10bc5da11674"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d7a4cd3-c4fa-4b45-ad85-51f80ac73945"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af231cc0-52ec-4047-881b-42ca56614d9d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""DropWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af3da0dc-73a7-4847-b3df-bae4c2cd857d"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""DropWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd561ccf-3a16-40cf-938d-1bbac0011770"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc6020f0-3dfe-4a96-bae7-3a2a3f766fc9"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""7940787f-bcc6-452f-b62e-506f4c27df94"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f5f1c31c-8f72-4836-8046-d976ef4af6ec"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""646cb1c9-6565-4827-b011-6460dd3e973b"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""16808930-514e-4442-b1a5-2a071ae8da89"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b1c92660-4e8f-4cca-85fe-d273f83055d6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b443e5f4-8e6b-4897-ad53-a0d22ac4cca4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftJoystick"",
                    ""id"": ""8758af0d-f983-4e7c-af9f-9bb004371c75"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5db8118c-21e8-41ca-8370-9f76104d9357"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""493f7c35-83af-40d1-9e73-2a5c7fb5b39f"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""78eecb8c-0e0c-4888-b99c-68b8307c8d9d"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""527a181d-38c1-45ba-ba91-318f5bc2fddc"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aaf9dd28-cbc0-425a-9051-228aa163b23f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb75f05e-e704-42be-a305-4eb39bff69a7"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d0f1e23-1cda-47c8-91d1-ad4b0453c9b5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""SelectPrevItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d66cbd79-592c-4c1c-bb92-5e888f575de9"",
                    ""path"": ""<XInputController>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectPrevItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8e6c352-3887-4df4-8235-de6fa8656932"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""SelectNextItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33c3ce34-116c-4e73-bab0-1b2c557613eb"",
                    ""path"": ""<XInputController>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectNextItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b05f325-0081-4b1a-a030-bb361f0d3606"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PickUpWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d5ab48f2-9fb4-4a0d-801a-f96791f6eadd"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PickUpWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b527c1d5-2793-4009-883f-4cf848766fcb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02be6168-5add-4566-be42-4da7f4f8d204"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Parry"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIControl"",
            ""id"": ""40a5159e-0713-4a31-9e70-5dd6cb04d725"",
            ""actions"": [
                {
                    ""name"": ""PopUpMenu"",
                    ""type"": ""Button"",
                    ""id"": ""91444265-4e70-4c16-b2b8-dc042b4c6c8d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0943db0f-541b-479c-b403-299fc7105e12"",
                    ""path"": ""*/{Menu}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PopUpMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerControl
        m_PlayerControl = asset.FindActionMap("PlayerControl", throwIfNotFound: true);
        m_PlayerControl_Move = m_PlayerControl.FindAction("Move", throwIfNotFound: true);
        m_PlayerControl_Jump = m_PlayerControl.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControl_Attack = m_PlayerControl.FindAction("Attack", throwIfNotFound: true);
        m_PlayerControl_DropWeapon = m_PlayerControl.FindAction("DropWeapon", throwIfNotFound: true);
        m_PlayerControl_SwitchWeapon = m_PlayerControl.FindAction("SwitchWeapon", throwIfNotFound: true);
        m_PlayerControl_Dash = m_PlayerControl.FindAction("Dash", throwIfNotFound: true);
        m_PlayerControl_UseItem = m_PlayerControl.FindAction("UseItem", throwIfNotFound: true);
        m_PlayerControl_SelectPrevItem = m_PlayerControl.FindAction("SelectPrevItem", throwIfNotFound: true);
        m_PlayerControl_SelectNextItem = m_PlayerControl.FindAction("SelectNextItem", throwIfNotFound: true);
        m_PlayerControl_PickUpWeapon = m_PlayerControl.FindAction("PickUpWeapon", throwIfNotFound: true);
        m_PlayerControl_Parry = m_PlayerControl.FindAction("Parry", throwIfNotFound: true);
        // UIControl
        m_UIControl = asset.FindActionMap("UIControl", throwIfNotFound: true);
        m_UIControl_PopUpMenu = m_UIControl.FindAction("PopUpMenu", throwIfNotFound: true);
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

    // PlayerControl
    private readonly InputActionMap m_PlayerControl;
    private IPlayerControlActions m_PlayerControlActionsCallbackInterface;
    private readonly InputAction m_PlayerControl_Move;
    private readonly InputAction m_PlayerControl_Jump;
    private readonly InputAction m_PlayerControl_Attack;
    private readonly InputAction m_PlayerControl_DropWeapon;
    private readonly InputAction m_PlayerControl_SwitchWeapon;
    private readonly InputAction m_PlayerControl_Dash;
    private readonly InputAction m_PlayerControl_UseItem;
    private readonly InputAction m_PlayerControl_SelectPrevItem;
    private readonly InputAction m_PlayerControl_SelectNextItem;
    private readonly InputAction m_PlayerControl_PickUpWeapon;
    private readonly InputAction m_PlayerControl_Parry;
    public struct PlayerControlActions
    {
        private InputController m_Wrapper;
        public PlayerControlActions(InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControl_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerControl_Jump;
        public InputAction @Attack => m_Wrapper.m_PlayerControl_Attack;
        public InputAction @DropWeapon => m_Wrapper.m_PlayerControl_DropWeapon;
        public InputAction @SwitchWeapon => m_Wrapper.m_PlayerControl_SwitchWeapon;
        public InputAction @Dash => m_Wrapper.m_PlayerControl_Dash;
        public InputAction @UseItem => m_Wrapper.m_PlayerControl_UseItem;
        public InputAction @SelectPrevItem => m_Wrapper.m_PlayerControl_SelectPrevItem;
        public InputAction @SelectNextItem => m_Wrapper.m_PlayerControl_SelectNextItem;
        public InputAction @PickUpWeapon => m_Wrapper.m_PlayerControl_PickUpWeapon;
        public InputAction @Parry => m_Wrapper.m_PlayerControl_Parry;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlActions instance)
        {
            if (m_Wrapper.m_PlayerControlActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnMove;
                Jump.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnJump;
                Attack.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack;
                Attack.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack;
                Attack.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnAttack;
                DropWeapon.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDropWeapon;
                DropWeapon.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDropWeapon;
                DropWeapon.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDropWeapon;
                SwitchWeapon.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSwitchWeapon;
                SwitchWeapon.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSwitchWeapon;
                SwitchWeapon.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSwitchWeapon;
                Dash.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDash;
                Dash.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDash;
                Dash.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnDash;
                UseItem.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnUseItem;
                UseItem.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnUseItem;
                UseItem.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnUseItem;
                SelectPrevItem.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectPrevItem;
                SelectPrevItem.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectPrevItem;
                SelectPrevItem.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectPrevItem;
                SelectNextItem.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectNextItem;
                SelectNextItem.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectNextItem;
                SelectNextItem.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnSelectNextItem;
                PickUpWeapon.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPickUpWeapon;
                PickUpWeapon.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPickUpWeapon;
                PickUpWeapon.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnPickUpWeapon;
                Parry.started -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnParry;
                Parry.performed -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnParry;
                Parry.canceled -= m_Wrapper.m_PlayerControlActionsCallbackInterface.OnParry;
            }
            m_Wrapper.m_PlayerControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Attack.started += instance.OnAttack;
                Attack.performed += instance.OnAttack;
                Attack.canceled += instance.OnAttack;
                DropWeapon.started += instance.OnDropWeapon;
                DropWeapon.performed += instance.OnDropWeapon;
                DropWeapon.canceled += instance.OnDropWeapon;
                SwitchWeapon.started += instance.OnSwitchWeapon;
                SwitchWeapon.performed += instance.OnSwitchWeapon;
                SwitchWeapon.canceled += instance.OnSwitchWeapon;
                Dash.started += instance.OnDash;
                Dash.performed += instance.OnDash;
                Dash.canceled += instance.OnDash;
                UseItem.started += instance.OnUseItem;
                UseItem.performed += instance.OnUseItem;
                UseItem.canceled += instance.OnUseItem;
                SelectPrevItem.started += instance.OnSelectPrevItem;
                SelectPrevItem.performed += instance.OnSelectPrevItem;
                SelectPrevItem.canceled += instance.OnSelectPrevItem;
                SelectNextItem.started += instance.OnSelectNextItem;
                SelectNextItem.performed += instance.OnSelectNextItem;
                SelectNextItem.canceled += instance.OnSelectNextItem;
                PickUpWeapon.started += instance.OnPickUpWeapon;
                PickUpWeapon.performed += instance.OnPickUpWeapon;
                PickUpWeapon.canceled += instance.OnPickUpWeapon;
                Parry.started += instance.OnParry;
                Parry.performed += instance.OnParry;
                Parry.canceled += instance.OnParry;
            }
        }
    }
    public PlayerControlActions @PlayerControl => new PlayerControlActions(this);

    // UIControl
    private readonly InputActionMap m_UIControl;
    private IUIControlActions m_UIControlActionsCallbackInterface;
    private readonly InputAction m_UIControl_PopUpMenu;
    public struct UIControlActions
    {
        private InputController m_Wrapper;
        public UIControlActions(InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @PopUpMenu => m_Wrapper.m_UIControl_PopUpMenu;
        public InputActionMap Get() { return m_Wrapper.m_UIControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIControlActions set) { return set.Get(); }
        public void SetCallbacks(IUIControlActions instance)
        {
            if (m_Wrapper.m_UIControlActionsCallbackInterface != null)
            {
                PopUpMenu.started -= m_Wrapper.m_UIControlActionsCallbackInterface.OnPopUpMenu;
                PopUpMenu.performed -= m_Wrapper.m_UIControlActionsCallbackInterface.OnPopUpMenu;
                PopUpMenu.canceled -= m_Wrapper.m_UIControlActionsCallbackInterface.OnPopUpMenu;
            }
            m_Wrapper.m_UIControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                PopUpMenu.started += instance.OnPopUpMenu;
                PopUpMenu.performed += instance.OnPopUpMenu;
                PopUpMenu.canceled += instance.OnPopUpMenu;
            }
        }
    }
    public UIControlActions @UIControl => new UIControlActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerControlActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDropWeapon(InputAction.CallbackContext context);
        void OnSwitchWeapon(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnSelectPrevItem(InputAction.CallbackContext context);
        void OnSelectNextItem(InputAction.CallbackContext context);
        void OnPickUpWeapon(InputAction.CallbackContext context);
        void OnParry(InputAction.CallbackContext context);
    }
    public interface IUIControlActions
    {
        void OnPopUpMenu(InputAction.CallbackContext context);
    }
}
