Public Class Physics
    Public Class particle
        Public position As New Vector(0, 0)
        Public velocity As New Vector(0, 0)
        Public acceleration As New Vector(0, 0)
        Public force As New Vector(0, 0)

        Public mass As Double = 3

        Public radius As Double = 1

        Public my_ellipse As New Ellipse
        Public reference_canvas As Canvas

        Public visible As Boolean = True

        Public Sub New(ByRef main_canvas As Canvas, ByVal start_position As Vector)
            reference_canvas = main_canvas

            position = start_position

            update_mass(mass)
            gui_update_size()
            my_ellipse.StrokeThickness = 0
            my_ellipse.Fill = Brushes.Black

            reference_canvas.Children.Add(my_ellipse)

            Canvas.SetLeft(my_ellipse, position.X - radius)
            Canvas.SetTop(my_ellipse, position.Y - radius)

        End Sub

        Public Sub make_invisible()
            my_ellipse.Visibility = Visibility.Hidden
            visible = False
        End Sub

        Public Sub update_mass(ByVal new_mass As Double)
            mass = new_mass

            radius = Math.Pow((new_mass / Math.PI) * (3.0 / 4.0), 1.0 / 3.0) * 5

        End Sub

        Public Sub gui_update_size()
            my_ellipse.Width = (radius * 2)
            my_ellipse.Height = (radius * 2)
        End Sub



        Public Sub physics_math(ByVal deltatime As Double)
            acceleration = Vector.Divide(force, mass)
            velocity = Vector.Add(velocity, Vector.Multiply(acceleration, deltatime))
            position = Vector.Add(position, Vector.Multiply(velocity, deltatime))

        End Sub

        Public Sub gui_update()
            Canvas.SetLeft(my_ellipse, position.X - radius)
            Canvas.SetTop(my_ellipse, position.Y - radius)
        End Sub

        Public Sub add_force(ByVal f As Vector)
            force = Vector.Add(force, f)
        End Sub

    End Class
End Class