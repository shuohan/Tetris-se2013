﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IDisplay
    {
        private TetrisGame _game;
        private Square[,] _image;
        private readonly Controller _controller;

        public MainWindow()
        {
            InitializeComponent();
            int[][,] styles = {new int[2, 2] {{1, 1}, {1, 1}}, new int[1,4]{{1,1,1,1}},  new int[2, 3] {{0, 0, 1}, {1, 1, 1}},new int[2,3]{{1,0,0},{1,1,1}},new int[2,3]{{0,1,0},{1,1,1}}};

            var game = new TetrisGame(Square.Styles(styles), new TimerEngine());
            game.AddDisplay(this);
            _controller=new Controller();
            game.SetController(_controller);
            game.Start();
        }

        private readonly Key[] Keys = new Key[]
        {
            Key.Left,
            Key.Right,
            Key.Up,
            Key.Down
        };

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keys.Contains(e.Key))
            {
                _controller.KeyState[e.Key] = true;
                if (e.Key == Key.Left) _controller.KeyState[Key.Right] = false;
                if (e.Key == Key.Right) _controller.KeyState[Key.Left] = false;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Keys.Contains(e.Key))
            {
               // _controller.KeyState[e.Key] = false;
            }
        }

        void IDisplay.OnStateChange()
        {
            _image = _game.Image;
            Console.Clear();
            Console.WriteLine("============");
            for (int i = 0; i < 15; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                    Console.Out.Write(_image[i, j] == null ? " " : "#");
                Console.Out.WriteLine("|");
            }
            Console.WriteLine("============");
        }

        void IDisplay.SetGame(TetrisGame game)
        {
            _game = game;
        }

        static readonly Dictionary<TetrisGame.GameAction,Key> Ht=new Dictionary<TetrisGame.GameAction, Key>()
        {
            {TetrisGame.GameAction.Left,Key.Left},
            {TetrisGame.GameAction.Right,Key.Right},
            {TetrisGame.GameAction.Down,Key.Down},
            {TetrisGame.GameAction.Rotate,Key.Up}
        };

        class Controller : IController
        {
            private Dictionary<Key,bool> _keyState=new Dictionary<Key, bool>()
            {
                {Key.Left,false},
                {Key.Right,false},
                {Key.Up,false},
                {Key.Down,false}
            };

            public Dictionary<Key, bool> KeyState
            {
                get { return _keyState; }
            }
            public bool Act(TetrisGame.GameAction action)
            {
                var result= KeyState[Ht[action]];
                KeyState[Ht[action]] = false;
                return result;
            }
        }
    }
}
