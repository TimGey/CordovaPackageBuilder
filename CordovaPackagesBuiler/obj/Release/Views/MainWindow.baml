   M S B A M L   `   `   `  ���� f  bFramework.Presentation.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=289ca9cbb8a2fa2azVclr-namespace:Framework.Presentation.Controls;assembly=Framework.Presentation.ControlsFramework.Presentation.Controls   CordovaPackagesBuiler,   &CordovaPackagesBuiler.Views.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  P LPrism.Wpf, Version=6.3.0.0, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59$prismhttp://prismlibrary.com/  econtrolsVclr-namespace:Framework.Presentation.Controls;assembly=Framework.Presentation.Controls  !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . $	��700��$	��600��$
��1000��$	��800��$	��700��$	��600��   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � Title �� *Title+.��� 
������� � RowDefinitions �� �� .+�3   -Framework.Presentation.Controls.BusyIndicator     BusyContent$ please wait��   IsBusy �� *IsBusy+.��� $��20q� �� �� �� �� �� �� �� �� �� ��  � ColumnDefinitions �� �� �� .+��� $��Choisissez la solution����  �� Orientation$ 
Horizontal=�$��1��$��3��.+��� $��
Rechercher��	 J� RenderTransformOrigin$	 
2.12,0.156&�$
��Auto��$��3q����� *SearchSolution+�� ���� *PathDirectory+�� $��2��$��3��$#��cochez le mode de déploiment���� $ 
Horizontal=�$��3��$��3��.+�	� $
��Prod��
 	� 	GroupName$
 
deploiment��$��	0,0,100,0q� d� 	IsChecked �� *Prod+	� $��Preprod��$
 
deploiment�� �� *	Preprod+�� $��4��$��3��$��cochez la platform���� $ 
Horizontal=�$��5��$��3��.+�	� $��5��$��Android��$
 platform��$��	0,0,100,0q� �� *	Android+	� $��5��$��Windows��$
 platform��$��	0,0,100,0q� �� *	Windows+�� $ 
Horizontal=�$��8��$��3��.+��� $��chemin final��$
��auto��$��3q����� *SelectFinalDirectoryPackages+�� ���� *PathFinalDirectory+�� $��9��$��3��$��	Générer��$
��Auto��$��1q����� *GeneratePackage+�� $��7��$��VersionCode���� $��7��$��1��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 130000�� �� TextWrapping$
 Wrap=����� *VersionCode+� $��6��$��3�� ��  �� �� �� �� .+��� $��1��$ 1.3.0.0��$
 Wrap=����� *VersionName+�� $��VersioneName���� $��2��$��Version Ident���� $��3��$
 1300��$
 Wrap=����� *VersionIdent+�� $��1�� �� AcceptsReturn . " �� VerticalScrollBarVisibility$
 Auto=�
���   �
�������$��Stretch=�$
 Wrap=�$��Stretch=����� *AffichageConsole+