using System;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Runner;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D Character;
    Texture2D OverWorld;

    // Текущие координаты положения персонажа
    Vector2 positionChar = Vector2.Zero;

    // Скорость движения персонажа
    float speed = 2f;

    // Ширина и высота фрейма персонажа
    int charFrameWidth = 15;
    int charFrameHeight = 22;

    // Текущий номер фрейма персонажа при движении вниз.
    Point charCurrentFrame = new Point (0, 0);

    // Размер спрайта (по 4 фрейма по ширине и высоте)
    Point charSpriteSize = new Point(4, 4);


    int currentTime = 0;    // Сколько времени прошло.
    int period = 100;        // пеиод обновления в миллисекундах.

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.ToggleFullScreen();
        _graphics.IsFullScreen = true;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        // Свойство TargetElapsedTime задает частоту смены кадров.
        // Последнее значение соответствует длительности игрового цикла - одного кадра.
        TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 1000/60);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Загружаем текстуру персонажа
        Character = Content.Load<Texture2D>("character");
        OverWorld = Content.Load<Texture2D>("Overworld");
    }

    protected override void Update(GameTime gameTime)

    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        {
            // Обработка движения персонажа Down, Up, Left, Right. 
            // Обработка ведется с учетом того, чтобы спрайт не выходил за экран.
            // Второе условие не дает "прыгнуть" спрайту за экран.

            if (Keyboard.GetState().IsKeyDown(Keys.Down) &
                positionChar.Y + speed <= Window.ClientBounds.Height - 22)
            {
                positionChar.Y += speed;

                // Добавляем к текущему времени прошедшее время
                currentTime += gameTime.ElapsedGameTime.Milliseconds;

                // Если текущее время превышает период обновления спрайта,
                // То обновляем спрайт для создания анимации
                if (currentTime > period)
                {
                    currentTime -= period; // "Обнуляем" наш текущий период обновления.
                    ++charCurrentFrame.X; // Переходим к следующему фрему в спрайте.

                    // После 4-ех фреймов переходим на первый.
                    if (charCurrentFrame.X >= charSpriteSize.X)
                        charCurrentFrame.X = 0;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) &
                positionChar.Y + speed >= Window.ClientBounds.Height - 22)
                    positionChar.Y = Window.ClientBounds.Height - 22;



            if (Keyboard.GetState().IsKeyDown(Keys.Up) &
            positionChar.Y > 0)

                if (positionChar.Y - speed <= 0)
                    positionChar.Y = 0;
                else positionChar.Y -= speed;


            if (Keyboard.GetState().IsKeyDown(Keys.Left) &
            positionChar.X > 0)

                if (positionChar.X - speed <= 0)
                    positionChar.X = 0;
                else positionChar.X -= speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) &
                positionChar.X <= Window.ClientBounds.Width - 15)

                if (positionChar.X + speed >= Window.ClientBounds.Width - 15)
                    positionChar.X = Window.ClientBounds.Width - 15;
                else positionChar.X += speed;
        }
        
        base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        for (int i=0; i<9; i++)
        {
            for (int j=0;j<9;j++)
            {
                _spriteBatch.Draw(OverWorld,
                new Vector2 (48*i, 
                48*j),
                new Rectangle (225,
                465,
                48,
                48),
                Color.White);
            }
        }

        _spriteBatch.Draw(Character,
        positionChar,
        new Rectangle(charCurrentFrame.X * 16,
        charCurrentFrame.Y * charFrameHeight + 7,
        charFrameWidth,
        charFrameHeight),
        Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}