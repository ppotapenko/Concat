   M S B A M L   `   `   `  ���� 
  Concat    Concat.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Icon$  	Icon1.ico�� � Title$ Concat��$	��657��$	��800��.��� $��Center=�$	��600��$��102,50,90,50q�.+��� -    ��Name$ TextBoxDirPath��$
��Left=�$��23��$��	35,54,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��386���� -    ��Name$ ButtonSelectFolder��$��Выбрать папку��$
��Left=�$��
432,54,0,0q�$	��Top=�$	��123��$��22���� -   $ ButtonAddToIgnore��$/��)Добавить папку в игнор��$
��Left=�$��432,219,0,0q�$	��Top=�$	��158��$��22���� -   $ TextBoxIgnoreFolders��$
��Left=�$��98��$��
35,219,0,0q�$	��Top=�$	��386��$��\.git;
\.svn;��$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=� �� AcceptsReturn . �� -   $ TextBoxFilterExt��$
��Left=�$��23��$��
35,135,0,0q�$
 Wrap=�$	��Top=�$	��555��$��cs���� -    ��Name$ LabelFilterExt��$]��WФильтр расширений файлов (пример ввода: cs, exe, doc)
��$
��Left=�$��
35,104,0,0q�$	��Top=�$	��555��$��26���� -   $ ButtonGetResult��$'��!Сформировать файл��$
��Left=�$��432,495,0,0q�$	��Top=�$	��158���� -   $ TextBoxFileTitle��$
��Left=�$��23��$��
35,447,0,0q�$
 Wrap=�$.��(Программный модуль:\n\n��$	��Top=�$	��555���� -	   $ LabelFileTitle��$c��]Текст заголовка для разделения содержимого файлов��$
��Left=�$��
35,416,0,0q�$	��Top=�$	��386���� -
   $ LabeldirPath��$5��/Путь к рабочей директории��$
��Left=�$��	35,28,0,0q�$	��Top=�$	��386���� -   $ LabelIgnoreFolders��$z��tИсключаемые папки в рабочей директории (пример ввода \.git; \packages;)��$
��Left=�$��
35,188,0,0q�$	��Top=�$	��555�� J� RenderTransformOrigin$ 0.523,-7.385&�$��26���� -   $ TextBoxGlobalIgnorFolders��$
��Left=�$��23��$��
35,369,0,0q�$
 Wrap=�$	��Top=�$	��555��$��
\bin, \obj���� -   $ LabelGlobalIgnoreFolders��$}��wИсключаемые папки во всех директориях (пример ввода: \bin, \obj, \Content)
��$
��Left=�$��
35,338,0,0q�$	��Top=�$	��555��$��26��