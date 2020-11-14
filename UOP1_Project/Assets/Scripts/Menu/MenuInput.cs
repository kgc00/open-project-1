using System;
using System.Collections;
using System.Collections.Generic;
using Menus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuInput : MonoBehaviour
{
	private GameObject _currentSelection;
	private GameObject _mouseSelection;
	[SerializeField] private InputReader _inputReader;
	public bool IsMouseActive { get; private set; }

	private void OnEnable()
	{
		_inputReader.Menu.MouseMoveMenuEvent += HandleMoveCursor;
		_inputReader.Menu.MoveSelectionMenuEvent += HandleMoveSelection;
	}

	private void OnDisable()
	{
		_inputReader.Menu.MouseMoveMenuEvent -= HandleMoveCursor;
		_inputReader.Menu.MoveSelectionMenuEvent -= HandleMoveSelection;
	}

	private void HandleMoveSelection()
	{
		DisableCursor();

		// occurs when mouse is on top of some button, and we hit a gamepad or keyboard key to change the selection
		var exitingMouseMode = EventSystem.current.currentSelectedGameObject == _currentSelection;

		if (exitingMouseMode)
		{
			_mouseSelection = _currentSelection;
			ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject,
				new PointerEventData(EventSystem.current),
				ExecuteEvents.pointerExitHandler);
			EventSystem.current.SetSelectedGameObject(_currentSelection);
		}
		else
		{
			_mouseSelection = null;
		}

		if (EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject(_mouseSelection);

		if (EventSystem.current.currentSelectedGameObject == null)
		{
			_mouseSelection = _currentSelection;
			EventSystem.current.SetSelectedGameObject(_currentSelection);
		}
	}

	private void DisableCursor()
	{
		Cursor.visible = false;
		IsMouseActive = false;
	}

	private void HandleMoveCursor()
	{
		if (_currentSelection != null && _mouseSelection != null &&
		    EventSystem.current.currentSelectedGameObject != null)
		{
			if (_mouseSelection == _currentSelection)
				EventSystem.current.SetSelectedGameObject(_mouseSelection);
		}

		EnableCursor();
	}

	private void EnableCursor()
	{
		Cursor.visible = true;
		IsMouseActive = true;
	}

	private void StoreSelection(GameObject uiElement)
	{
		EventSystem.current.SetSelectedGameObject(uiElement);
		_currentSelection = uiElement;
	}

	public void HandleMouseEnter(GameObject uiElement)
	{
		StoreSelection(uiElement);
	}

	public void HandleMouseExit(GameObject uiElement)
	{
		if (EventSystem.current.currentSelectedGameObject == uiElement)
		{
			_mouseSelection = uiElement;
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
