   M S B A M L   `   `   `  ���� 
  Concat    Concat.ProgressWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��Window��  � Title$%  Обработка файлов��$	��110��$	��300�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=� � Topmost . .���� $��2q�
�������._��  � RowDefinitions �� $��32 ��� $��* ��� $��42 �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Bottom=�.+��� -    ��Name$ TextBlockProcessing��$��16(�Найдено файлов:�� -   $ TextBlockCurrent��$y�Hidden=�$��16(�$��5,0,0,0q�����  �� Path$ Current� �� ElementName$ Window��	 �� Mode$	 OneWay=��� -   $ TextBlockOf��$y�Hidden=�$��16(�$��5,0,5,0q�из�� -   $ TextBlockTotal��$��16(����� $ Total�$ Window��$	 OneWay=�� $��1��$	��200��$��20��
 � Minimum$
 0Q� � Value �� $ Current�$ Window��$	 OneWay=� � Maximum �� $ Total�$ Window��$	 OneWay=��� $��2��$ 
Horizontal=�$��Center=��� -    ��Name$ ButtonCancel��$��
190,6,10,6q�$��30��$��Отмена��$��2���� -   $ ButtonStart��$��
10,6,190,6q�$��30��$��Обработать��$��2��