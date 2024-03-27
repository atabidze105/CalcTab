using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace CalcTab
{
    public partial class MainWindow : Window
    {
        string _numberNow = ""; //поле для второго операнда
        string _numberPr = ""; //Поле для первого операнда
        public MainWindow()
        {
            InitializeComponent();
        }

        public void inputNumber(object? sender, RoutedEventArgs args) //Ввод и удаление последнего символа
        {
            var button = (sender as Button)!;
            text.Text = text.Text == "0" || text.Text == _numberPr ? "" : text.Text;
            switch (button.Name)
            {
                case "But_1":
                    text.Text += "1";
                    break;
                case "But_2":
                    text.Text += "2";
                    break;
                case "But_3":
                    text.Text += "3";
                    break;
                case "But_4":
                    text.Text += "4";
                    break;
                case "But_5":
                    text.Text += "5";
                    break;
                case "But_6":
                    text.Text += "6";
                    break;
                case "But_7":
                    text.Text += "7";
                    break;
                case "But_8":
                    text.Text += "8";
                    break;
                case "But_9":
                    text.Text += "9";
                    break;
                case "But_0":
                    text.Text += "0";
                    break;
                case "But_pi":
                    text.Text = Convert.ToString(Math.PI);
                    break;
                case "But_euler":
                    text.Text = Convert.ToString(Math.E);
                    break;
                case "But_back"://Удаление последнего символа
                    string numm = text.Text;
                    if (text.Text != "")
                    {
                        numm = numm.Remove(numm.Length - 1);
                        text.Text = numm;
                    }
                    break;
            }
            text.Text = text.Text == "" || text.Text == "-" ? "0" : text.Text; //если строка пуста или при стирании числа остался минус, отображается ноль
        }

        public void inputOther(object? sender, RoutedEventArgs args) //Стирание строк и кнопка запятой
        {
            var button = (sender as Button)!;
            int dotCount = 0; //счетчик точек
            switch (button.Name)
            {
                case "But_CE":
                    {
                        text.Text = "0";
                    }
                    break;
                case "But_Clr":
                    {
                        text.Text = "0";
                        previously.Text = "";
                    }
                    break;
                case "But_fr": //Использование этой кнопки удалит запятую или добавит ее в конце строки //Если добавить запятую после нуля, а затем добавить другие цифры и еще раз использовать кнопку (уже для удаления запятой), 0 в начале сохранится (врочем, на корректность счета это не влияет)
                    {
                        if (text.Text.Contains(',') == true)
                        {
                            text.Text = text.Text.Replace(",", "");
                            dotCount++;
                        }
                        if (dotCount == 0) //Добавление запятой
                        {
                            text.Text += text.Text == "" ? "0," : ","; //т.к. в большой строке "0" == "", исп-е кнопки заменит ее на "0,"
                        }
                    }
                    break;
            }
        }

        public void operation(object? sender, RoutedEventArgs args) //основные математические операции и равно
        {
            if (previously.Text == "")
            {
                var button = (sender as Button)!;
                _numberPr = text.Text;
                switch (button.Name) //Вычислления происходят в методе equal(), здесь только меняется малая строка, конечный символ которой влияет на выполнение операции 
                {
                    case "But_plus":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")+" : text.Text + "+";
                        }
                        break;
                    case "But_min":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")-" : text.Text + "-";
                        }
                        break;
                    case "But_mult":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")*" : text.Text + "*";
                        }
                        break;
                    case "But_slash":
                        {
                            previously.Text = text.Text.StartsWith('-') == true ? "(" + text.Text + ")/" : text.Text + "/";
                        }
                        break;
                    case "But_equal":
                        {
                            if (previously.Text != "")
                            {
                                _numberNow = _numberPr;
                                equal();
                            }
                        }
                        break;
                }
            }
            else //Если в малой строке есть символы, то нажатие на любую из кнопок операций вызовет метод equal()
            {
                var button = (sender as Button)!;
                switch (button.Name)
                {
                    default:
                        {
                            _numberNow = text.Text;
                            equal();
                        }
                        break;
                }
            }
        }

        private void symChange(object? sender, Avalonia.Interactivity.RoutedEventArgs e) //смена знака перед числом
        {
            text.Text = text.Text == "0" ? text.Text : (text.Text.StartsWith('-') ? text.Text.Remove(0, 1) : '-' + text.Text);
        }

        private void persent(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_numberPr == "")
            {
                text.Text = Convert.ToString(Convert.ToDouble(text.Text) / 1000); //При нажатии на "%" с пустой малой строкой выполняется некорректное вычисление одного процента (одна тысячная вместо одной сотой)
            }
            else
            {
                if (previously.Text != "")
                {
                    double numFull = Convert.ToDouble(text.Text);
                    _numberNow = Convert.ToString(numFull / 100);
                    text.Text = Convert.ToString(_numberNow);
                }
                else
                {
                    double numFull = Convert.ToDouble(_numberPr);
                    _numberNow = Convert.ToString(numFull / 100);
                    text.Text = Convert.ToString(_numberNow);
                }
            }
        }

        private double factorial(double number) //метод для факториала
        {
            double numReturn = 1;
            if (number > 0)
            {
                for (int i = 1; i <= number; i++)
                {
                    numReturn *= i;
                }
                return numReturn;
            }
            else if (number < 0)
            {
                for (int i = -1; i >/*=*/ number; i--) //при использовании факториал не дойдет до корректного значения из-за потери последнего множителя
                {
                    numReturn *= i;
                }
                return numReturn;
            }
            else
            {
                return 1;
            }
        }

        public void function(object? sender, RoutedEventArgs args) //Функции применяются только к тому, что находится в большой строке
        {
            var button = (sender as Button)!;
            switch (button.Name)
            {
                case "But_drob":
                    previously.Text = "1/(" + text.Text + ")";
                    text.Text = Convert.ToString(1 / Convert.ToDouble(text.Text));
                    break;
                case "But_Sq":
                    previously.Text = "sqr(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Pow(Convert.ToDouble(text.Text), 2));
                    break;
                case "But_Sqrt":
                    previously.Text = "sqrt(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(text.Text)));
                    break;
                case "But_sin": //Все следующие триготометрические функции актуальны только для градусов
                    previously.Text = "sin(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sin(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_sinh":
                    previously.Text = "sinh(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Sinh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_cos":
                    previously.Text = "cos(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Cos(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_cosh":
                    previously.Text = "cosh(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Cosh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_tan":
                    previously.Text = "tan(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Tan(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_tanh":
                    previously.Text = "tanh(" + text.Text + ")"; text.Text = Convert.ToString(Math.Tanh(Math.PI * Convert.ToDouble(text.Text) / 180));
                    break;
                case "But_ln":
                    previously.Text = "ln(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Log(Convert.ToDouble(text.Text)));
                    break;
                case "But_log":
                    previously.Text = "log(" + text.Text + ")";
                    text.Text = Convert.ToString(Math.Log10(Convert.ToDouble(text.Text)));
                    break;
                case "But_fact":
                    previously.Text = "fact(" + text.Text + ")";
                    text.Text = Convert.ToString(factorial(Convert.ToDouble(text.Text)));
                    break;
            }

            previously.Text = text.Text == "∞" || text.Text == "-∞" || text.Text == "не число" ? "" : previously.Text;
            text.Text = text.Text == "∞" || text.Text == "-∞" || text.Text == "не число" ? "0" : text.Text;
        }

        private void equal()
        {
            double num1 = Convert.ToDouble(_numberPr);
            double num2 = Convert.ToDouble(_numberNow);
            switch (previously.Text.Substring(previously.Text.Length - 1)) //В зависимости от последнего символа в малой строке будет выполнена одна из следующих операций
            {
                case "+":
                    text.Text = Convert.ToString(num1 + num2);
                    break;
                case "-":
                    text.Text = Convert.ToString(num1 - num2);
                    break;
                case "*":
                    text.Text = Convert.ToString(num1 * num2);
                    break;
                case "/":
                    text.Text = Convert.ToString(num1 / num2);
                    break;
            }
            previously.Text = "";
            _numberPr = "";
        }
    }

}