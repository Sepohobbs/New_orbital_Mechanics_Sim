Imports System.Threading

Class MainWindow
    Dim thread_running As Boolean = False
    Dim particles As New List(Of Physics.particle)

    Dim frame_rate As Double = 100


    Dim sim_speed As Double = 2
    Dim g_constant As Double = 17

    Dim rand As New Random

    Dim current_p_size As Integer

    'IDEAS:
    'click and drag to choose initial direction and velocity. This will be hard
    'colors based on size. starts as rocky planet, then gas giant, then star. Maybe even a black hole
    'far off goal, have a button that places a solor system already then you can shoot starts into it! 
    'could even make explosion or supernova by turning 'current_p_size' negative for a moment


    'BUGS
    'fix velocity issue. Momentum does not cancel out, instead it seems to just adopt fastest moving particle
    
    Private Sub Canvas1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Canvas1.MouseDown

        Dim Point_p = Mouse.GetPosition(Canvas1)

        Dim start_position As New Vector(Point_p.X, Point_p.Y)
        Dim p As New Physics.particle(Canvas1, start_position)

        p.mass = p.mass + current_p_size
        p.update_mass(p.mass)
        p.gui_update_size()
        p.gui_update()

        particles.Add(p)

        TextBox1.Text = Point_p.ToString

    End Sub


    Public Sub run_engine()
        While thread_running
            For i = 0 To particles.Count - 1
                If particles(i).visible Then
                    For j = i To particles.Count - 1
                        If i <> j And particles(j).visible Then
                            Dim diff_vector_i As Vector = Vector.Subtract(particles(j).position, particles(i).position)
                            Dim diff_vector_j As Vector = Vector.Subtract(particles(i).position, particles(j).position)
                            Dim mass_element As Double = particles(i).mass * particles(j).mass

                            If diff_vector_i.Length < particles(i).radius + particles(j).radius Then
                                If particles(i).mass > particles(j).mass Then
                                    particles(i).update_mass(particles(i).mass + particles(j).mass)
                                    Dispatcher.Invoke(Sub() particles(i).gui_update_size())
                                    Dispatcher.Invoke(Sub() particles(j).make_invisible())

                                    particles(i).force += particles(j).force
                                    'particles(i).velocity += particles(j).velocity
                                    particles(i).velocity = particles(i).velocity + (particles(j).velocity * (particles(j).mass / particles(i).mass))

                                Else
                                    particles(j).update_mass(particles(j).mass + particles(i).mass)
                                    Dispatcher.Invoke(Sub() particles(j).gui_update_size())
                                    Dispatcher.Invoke(Sub() particles(i).make_invisible())

                                    particles(j).force += particles(i).force
                                    'particles(j).velocity += particles(i).velocity
                                    particles(j).velocity = particles(j).velocity + (particles(i).velocity * (particles(i).mass / particles(j).mass))
                                End If


                            Else
                                Dim force_amount As Double = g_constant * mass_element / diff_vector_i.LengthSquared
                                diff_vector_i.Normalize()
                                diff_vector_i = Vector.Multiply(force_amount, diff_vector_i)

                                diff_vector_j.Normalize()
                                diff_vector_j = Vector.Multiply(force_amount, diff_vector_j)

                                particles(i).add_force(diff_vector_i)
                                particles(j).add_force(diff_vector_j)

                            End If

                            End If

                    Next
                End If
            Next


            For p = 0 To particles.Count - 1
                particles(p).physics_math((1 / frame_rate) * sim_speed)
                Dispatcher.Invoke(Sub() particles(p).gui_update())
            Next
            Thread.Sleep(1000 / frame_rate)

        End While

    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        'Dim start_position As New Vector(258, 133)
        'Dim p As New Physics.particle(Canvas1, start_position)

        'p.update_mass(p.mass + 10)
        'p.gui_update_size()
        'p.gui_update()

        'particles.Add(p)


        'Dim start_position2 As New Vector(350, 133)
        'Dim p2 As New Physics.particle(Canvas1, start_position2)

        'p2.mass = p2.mass + current_p_size
        'p2.update_mass(p2.mass + 30)
        'p2.gui_update_size()
        'p2.gui_update()

        'particles.Add(p2)

    End Sub

    Private Sub play_bttn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles play_bttn.Click

        thread_running = True
        Dim t As New Thread(AddressOf run_engine)
        t.Start()
    End Sub


    Private Sub pause_bttn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles pause_bttn.Click


        thread_running = False
        Dim t As New Thread(AddressOf run_engine)


    End Sub

   
   
    Private Sub p_size_up_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles p_size_up.Click
        current_p_size = current_p_size + 2
        p_size_txt.Text = current_p_size

    End Sub

    Private Sub p_size_down_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles p_size_down.Click
        If current_p_size > 0 Then
            current_p_size = current_p_size - 2
            p_size_txt.Text = current_p_size
        End If
    End Sub





    Private Sub Window_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        thread_running = False
    End Sub
End Class
