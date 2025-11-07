/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   executor_status.c                                  :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/14 20:54:16 by caide-so          #+#    #+#             */
/*   Updated: 2025/06/06 11:08:25 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	set_last_status(t_shell *shell, int status)
{
	if (g_signal_status >= 0)
	{
		update_sh_last_status(shell, -2);
		return ;
	}
	shell->last_status = status & 0xFF;
}

int	get_last_status(t_shell *shell)
{
	return (shell->last_status);
}
