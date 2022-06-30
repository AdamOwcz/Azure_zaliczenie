using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Device.Gpio;
 
namespace Clock
{
    class Program
    {
 
        //Ras
 
    static bool isTimerRuning;
    private static Stopwatch watch = new Stopwatch();
 
    static void Main(string[] args)
    {
 
    const int LED_PIN = 18;
    const int LED_PIN_2 = 12;
    const int LED_PIN_3 = 21;
    const int PIR_PIN = 23;
 
    using var controller = new GpioController();
    controller.OpenPin(LED_PIN, PinMode.Output);
    controller.OpenPin(LED_PIN_2, PinMode.Output);
    controller.OpenPin(LED_PIN_3, PinMode.Output);
    controller.OpenPin(PIR_PIN, PinMode.Input);
 
    controller.RegisterCallbackForPinValueChangedEvent(PIR_PIN, PinEventTypes.Rising, (sender, args) =>
    {
    
        //Console.WriteLine("LED is ON V3");
    
    });

    controller.RegisterCallbackForPinValueChangedEvent(PIR_PIN, PinEventTypes.Falling, (sender, args) =>
    {
        //Console.WriteLine("LED is OFF V3");
    });

            //Menu
            ShowMenu();
            CancellationTokenSource cts = new CancellationTokenSource();
            var task = new Task(() => ShowTheWatchInLineNumber18(cts));
            task.Start();
 
 
            while (!task.IsCompleted)
            {
                var keyInput = Console.ReadKey(true);
                if (!Console.KeyAvailable)
                {
                    if (keyInput.Key == ConsoleKey.Spacebar)
 
                    {
                        if (isTimerRuning)
                        {
                            watch.Stop();
                            Console.ForegroundColor = ConsoleColor.Red;
                            ShowMessageInLineNumber16("Spacebar pressed: Timer is stoped.");
                            controller.Write(LED_PIN, PinValue.Low);
                            controller.Write(LED_PIN_2, PinValue.High);
                            controller.Write(LED_PIN_3, PinValue.Low);                           
                        }
                        else
                        {
                            watch.Start();
                            Console.ForegroundColor = ConsoleColor.Green;
 
                            ShowMessageInLineNumber16("Spacebar pressed: Timer is running.");       
                                controller.Write(LED_PIN, PinValue.High);
                                controller.Write(LED_PIN_2, PinValue.Low);
                                controller.Write(LED_PIN_3, PinValue.Low);
    
                        }
                        isTimerRuning = !isTimerRuning;
                    }
 
                    else if (keyInput.Key == ConsoleKey.R)
                    {
                        isTimerRuning = false;
                        watch.Reset();
                        Console.ForegroundColor = ConsoleColor.Green;
                        ShowMessageInLineNumber16("R pressed: Reset timer in 7s.");
                        controller.Write(LED_PIN_3, PinValue.High);
                        controller.Write(LED_PIN_2, PinValue.Low);
                        controller.Write(LED_PIN_2, PinValue.Low);
                        Task.Delay(7000).Wait();
                        ShowMenu();
                    }
 
                    else
                    if (keyInput.Key == ConsoleKey.Escape)
                    {
                        cts.Cancel();
                        ShowMenu();
                        ShowMessageInLineNumber16("ESC pressed: Exiting program in 1s.");
                        Task.Delay(1000).Wait();
                        break;
                    }
  
                    Task.Delay(35).Wait();
                    Console.WriteLine("Awaiting for user action..."); 
                }
            }
        }
       
        private static void ShowMenu()
        {
            Console.Clear();
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("=============Zjezdzalnia================");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("{0,-10} to reset timer in 1s.", "[R]");
            Console.WriteLine("{0,-10} to quit the program in 1s.", "[ESC]");
            Console.WriteLine();
            Console.WriteLine("{0,-10} to start/pause the watch.", "[SPACE]");
        }
    
        private static void ShowMessageInLineNumber16(string mess)
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine("{0,-100}", mess);
        }
 
        static void ShowTheWatchInLineNumber18(CancellationTokenSource _cts)
        {
            int minuteInOneHour = 60;
            int secondInOneMinute = 60;
            int milisecondInOneSecond = 1000;
 
            Task.Delay(1).Wait();
            while (!_cts.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 18);
                if (isTimerRuning)
                {
                    if (watch.ElapsedMilliseconds != 0)
                    {
                        var minute = (watch.ElapsedMilliseconds / (secondInOneMinute* milisecondInOneSecond)) % minuteInOneHour;
                        var sec = (watch.ElapsedMilliseconds / milisecondInOneSecond) % secondInOneMinute;
                        var miliSec = watch.ElapsedMilliseconds % milisecondInOneSecond;
 
                        Console.WriteLine("{0,2:0#}:{1,2:0#}:{2,-100:0##}", minute, sec, miliSec);
                    }
                }
                Task.Delay(1).Wait();
            }
        }
    }
}